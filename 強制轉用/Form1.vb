Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Sockets
Imports System.Threading


Public Class Form1
    ' Telnet 連線相關變數
    Private T_Stream As Net.Sockets.NetworkStream
    Private T_Client As New Net.Sockets.TcpClient()
    Private bytWriting As Byte()
    Private bytReading As Byte()

    ' 程序追蹤變數
    Private Connected As Boolean = False
    Private SearchResults As Integer = 0
    Private LastSearchedCoil As String = ""
    Private LastLockedIndex As Integer = -1
    Private ModifyMode As Boolean = False
    Private IsModifyLocked As Boolean = False ' 新增: 是否已送出 MODIFY 指令
    Private big5Encoding As Encoding

    ' 表格資料暫存
    Private currentDataRows As New List(Of String())

    ' 帳號與資料表對應
    Private accountMapping As New Dictionary(Of String, String) From {
        {"D1M65050-DOM", "T16510"},
        {"D1M65150-DOM", "T16510"}
    }

    ' 表格與鋼捲編號長度對應
    Private coilTableMapping As New Dictionary(Of Integer, String()) From {
        {7, New String() {"D1M65050-DOM"}},
        {8, New String() {"D1M65150-DOM"}}
    }

    ' 界面初始化
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 設置 LogTextbox 為只讀
        LogTextbox.ReadOnly = True

        ' 初始化界面狀態
        UpdateUIState(False)

        ' 初始化 DataGridView
        InitializeDataGridView()

        ' 設置 Coil_tbx 欄位驗證 
        Coil_tbx.MaxLength = 8

        ' 註冊編碼提供者
        big5Encoding = Encoding.GetEncoding("BIG5")
    End Sub

    ' 初始化 DataGridView
    Private Sub InitializeDataGridView()
        DataGridView1.Columns.Clear()

        ' 添加表格列
        DataGridView1.Columns.Add("Index", "序號")
        DataGridView1.Columns.Add("Field", "欄位")
        DataGridView1.Columns.Add("Value", "值")

        ' 設定表格屬性
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.ReadOnly = True
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    ' 更新界面狀態
    Private Sub UpdateUIState(isConnected As Boolean)
        Connected = isConnected

        ' 連接時的界面狀態
        Login_btn.Enabled = Not isConnected Or isConnected
        Login_btn.Text = If(isConnected, "再次搜尋", "第一次搜尋及登入")

        ' 其他按鈕需要連接後才能啟用
        lock_btn.Enabled = isConnected And SearchResults > 0

        ' 修改相關按鈕狀態 - 考慮修改模式
        If ModifyMode Then
            ' 在修改模式下，鎖定修改相關控件直到修改完成或取消
            Modify_cb.Enabled = False
            Modify_tbx.Enabled = False
            Confirm_btn.Enabled = False
            Cancel_btn.Enabled = True  ' 在修改模式下，只有取消按鈕是啟用的
        ElseIf IsModifyLocked Then
            ' 進入修改鎖定狀態，僅允許輸入新值與確認
            Modify_cb.Enabled = False
            Modify_tbx.Enabled = True
            Confirm_btn.Enabled = True
            Cancel_btn.Enabled = True
        Else
            ' 非修改模式下，根據是否鎖定數據決定控件狀態
            Modify_cb.Enabled = isConnected And LastLockedIndex >= 0
            Modify_tbx.Enabled = False
            Confirm_btn.Enabled = False
            Cancel_btn.Enabled = False
        End If

        ' 恢復按鈕需要連接且有數據
        LockModify_btn.Enabled = isConnected And DataGridView1.Rows.Count > 0 AndAlso Modify_cb.SelectedIndex >= 0
    End Sub

    ' 鋼捲編號變更事件
    Private Sub Coil_tbx_TextChanged(sender As Object, e As EventArgs) Handles Coil_tbx.TextChanged
        Dim length = Coil_tbx.Text.Length

        ' 清空表格選項
        Table_CB.Items.Clear()

        ' 根據長度自動選擇資料表
        If length >= 7 AndAlso length <= 8 Then
            If coilTableMapping.ContainsKey(length) Then
                Table_CB.Items.AddRange(coilTableMapping(length))
                If Table_CB.Items.Count > 0 Then
                    Table_CB.SelectedIndex = 0
                End If
            End If
        End If

        ' 驗證鋼捲編號長度
        If length < 7 OrElse length > 8 Then
            Coil_tbx.BackColor = Color.LightPink
            Login_btn.Enabled = False
        ElseIf Not IsNumeric(Coil_tbx.Text) Then
            Coil_tbx.BackColor = Color.LightPink
            Login_btn.Enabled = False
        Else
            Coil_tbx.BackColor = Color.White
            Login_btn.Enabled = True
        End If
    End Sub

    ' 限制鋼捲編號只能輸入數字
    Private Sub Coil_tbx_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Coil_tbx.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' 資料表選擇變更事件
    Private Sub Table_CB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Table_CB.SelectedIndexChanged
        ' 根據選擇的資料表自動設置帳號
        If Table_CB.SelectedIndex >= 0 Then
            Dim selectedTable As String = Table_CB.SelectedItem.ToString()
            If accountMapping.ContainsKey(selectedTable) Then
                Acc_TB.Text = accountMapping(selectedTable)
            End If
        End If
    End Sub

    ' 登入按鈕點擊事件
    Private Sub Login_btn_Click(sender As Object, e As EventArgs) Handles Login_btn.Click
        ' 驗證輸入
        If String.IsNullOrEmpty(Coil_tbx.Text) OrElse Coil_tbx.Text.Length < 7 OrElse Coil_tbx.Text.Length > 8 Then
            MessageBox.Show("請輸入7-8碼的鋼捲編號", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrEmpty(Table_CB.Text) Then
            MessageBox.Show("請選擇資料表", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' 設置界面
        Cursor = Cursors.WaitCursor
        LogTextbox.Clear()
        DataGridView1.Rows.Clear()
        SearchRes_LB.Text = "搜尋結果：? 筆"
        currentDataRows.Clear()

        ' 執行登入和搜尋
        If Connected Then
            ' 如果已連線，則只執行搜尋
            PerformSearch()
        Else
            ' 執行登入流程
            PerformLogin()
        End If

        Cursor = Cursors.Default
    End Sub

    ' 執行登入流程
    Private Sub PerformLogin()
        Try
            ' 首先關閉之前可能未完全清理的連線
            CloseConnection()

            ' 連接 Telnet 服務器
            T_Client = New TcpClient()
            T_Client.SendTimeout = 3000  ' 增加超時時間
            T_Client.ReceiveTimeout = 3000  ' 增加接收超時時間
            LogTextbox.AppendText("正在連接到服務器 16.168.22.8:23..." & vbCrLf)
            T_Client.Connect("16.168.22.8", 23)
            T_Stream = T_Client.GetStream()

            ' 等待服務器響應（增加等待時間）
            Thread.Sleep(2000)
            LogTextbox.AppendText("連接成功，等待服務器響應..." & vbCrLf)

            ' 讀取登入提示 - 使用增強的讀取方法
            Dim response = ReadDataWithRetry()
            LogTextbox.AppendText(response & vbCrLf)

            ' 如果看到用戶名輸入提示，輸入用戶名
            If response.Contains("Username:") OrElse response.Contains("login:") Then
                LogTextbox.AppendText("檢測到用戶名提示，正在登入..." & vbCrLf)
                WriteData(Acc_TB.Text & vbCrLf)
                Thread.Sleep(1000)
                response = ReadDataWithRetry()
                LogTextbox.AppendText(response & vbCrLf)

                ' 如果看到密碼輸入提示，輸入密碼
                If response.Contains("Password:") Then
                    LogTextbox.AppendText("檢測到密碼提示，正在輸入密碼..." & vbCrLf)
                    WriteData("666666" & vbCrLf)
                    Thread.Sleep(1500)
                    response = ReadDataWithRetry()
                    LogTextbox.AppendText(response & vbCrLf)

                    ' 處理登入後的提示
                    ProcessLoginPrompts()

                    ' 設置連接標志
                    Connected = True

                    ' 搜尋鋼捲
                    PerformSearch()

                    ' 更新界面
                    UpdateUIState(Connected)
                Else
                    LogTextbox.AppendText("未收到密碼提示，可能是用戶名錯誤或服務器響應異常" & vbCrLf)
                End If
            Else
                LogTextbox.AppendText("未收到用戶名提示，進行第二次嘗試..." & vbCrLf)
                ' 嘗試按Enter看是否能觸發登入提示
                WriteData(vbCrLf)
                Thread.Sleep(1000)
                response = ReadDataWithRetry()
                LogTextbox.AppendText(response & vbCrLf)

                ' 再次檢查用戶名提示
                If response.Contains("Username:") OrElse response.Contains("login:") Then
                    LogTextbox.AppendText("第二次嘗試檢測到用戶名提示，正在登入..." & vbCrLf)
                    WriteData(Acc_TB.Text & vbCrLf)
                    Thread.Sleep(1000)
                    response = ReadDataWithRetry()
                    LogTextbox.AppendText(response & vbCrLf)

                    ' 如果看到密碼輸入提示，輸入密碼
                    If response.Contains("Password:") Then
                        LogTextbox.AppendText("檢測到密碼提示，正在輸入密碼..." & vbCrLf)
                        WriteData("666666" & vbCrLf)
                        Thread.Sleep(1500)
                        response = ReadDataWithRetry()
                        LogTextbox.AppendText(response & vbCrLf)

                        ' 處理登入後的提示
                        ProcessLoginPrompts()

                        ' 設置連接標志
                        Connected = True

                        ' 搜尋鋼捲
                        PerformSearch()

                        ' 更新界面
                        UpdateUIState(Connected)
                    Else
                        LogTextbox.AppendText("未收到密碼提示，可能是用戶名錯誤或服務器響應異常" & vbCrLf)
                    End If
                Else
                    LogTextbox.AppendText("連接成功但未收到用戶名提示，請檢查服務器狀態" & vbCrLf)
                End If
            End If
        Catch ex As Exception
            LogTextbox.AppendText("連接出錯: " & ex.Message & vbCrLf)
            LogTextbox.AppendText("嘗試延長等待時間重新連接..." & vbCrLf)
            Try
                ' 關閉現有連接
                CloseConnection()

                ' 再次嘗試連接，增加更長的等待時間
                T_Client = New TcpClient()
                T_Client.SendTimeout = 5000
                T_Client.ReceiveTimeout = 5000
                LogTextbox.AppendText("正在重新連接到服務器 16.168.22.8:23..." & vbCrLf)
                T_Client.Connect("16.168.22.8", 23)
                T_Stream = T_Client.GetStream()

                ' 等待更長時間
                Thread.Sleep(3000)
                LogTextbox.AppendText("重新連接成功，等待服務器響應..." & vbCrLf)

                ' 讀取登入提示 - 使用增強的讀取方法
                Dim response = ReadDataWithRetry(maxRetries:=3)
                LogTextbox.AppendText(response & vbCrLf)

                If response.Contains("Username:") OrElse response.Contains("login:") Then
                    LogTextbox.AppendText("重新連接後檢測到用戶名提示，正在登入..." & vbCrLf)
                    ' 繼續正常登入流程
                    WriteData(Acc_TB.Text & vbCrLf)
                    Thread.Sleep(1000)
                    response = ReadDataWithRetry()
                    LogTextbox.AppendText(response & vbCrLf)

                    If response.Contains("Password:") Then
                        WriteData("666666" & vbCrLf)
                        Thread.Sleep(1500)
                        response = ReadDataWithRetry()
                        LogTextbox.AppendText(response & vbCrLf)

                        ProcessLoginPrompts()
                        Connected = True
                        PerformSearch()
                        UpdateUIState(Connected)
                    End If
                Else
                    LogTextbox.AppendText("重新連接後仍未收到用戶名提示，請檢查網絡或服務器狀態" & vbCrLf)
                End If
            Catch retryEx As Exception
                LogTextbox.AppendText("重新連接失敗: " & retryEx.Message & vbCrLf)
                CloseConnection()
            End Try
        End Try
    End Sub

    ' 增強的讀取數據方法，帶有重試機制
    Private Function ReadDataWithRetry(Optional maxRetries As Integer = 2) As String
        Dim sData As String = ""
        Dim retryCount As Integer = 0

        While retryCount < maxRetries
            Try
                Dim partialData = ReadData()
                sData += partialData

                ' 如果讀取到了有意義的數據，可以退出循環
                If Not String.IsNullOrWhiteSpace(partialData) Then
                    Exit While
                End If

                ' 沒有讀到數據，等待一下再試
                Thread.Sleep(500)
                retryCount += 1
            Catch ex As Exception
                LogTextbox.AppendText("讀取重試 " & (retryCount + 1) & " 失敗: " & ex.Message & vbCrLf)
                retryCount += 1
                Thread.Sleep(500)
            End Try
        End While

        Return sData.Trim()
    End Function

    ' 處理登入後的提示
    Private Sub ProcessLoginPrompts()
        ' 處理可能的提示，如系統消息等
        Dim response As String

        ' 檢查是否有提示檢查錯誤文件
        LogTextbox.AppendText("處理登入後的提示..." & vbCrLf)
        response = ReadDataWithRetry()
        LogTextbox.AppendText(response & vbCrLf)

        If response.Contains("Check D1T00060_ERR ?(Y/N):") Then
            WriteData("N" & vbCrLf) ' 輸入N跳過，替代原本的Enter
            Thread.Sleep(500)
            response = ReadDataWithRetry()
            LogTextbox.AppendText(response & vbCrLf)
        End If

        ' 無論命令提示符是什麼，都執行初始化命令
        LogTextbox.AppendText("執行必要的初始化命令序列..." & vbCrLf)

        ' 發送回車鍵，確保提示符出現
        WriteData(vbCrLf)
        Thread.Sleep(500)

        ' 執行 ULG 命令
        LogTextbox.AppendText("執行 ULG 命令..." & vbCrLf)
        WriteData("ULG" & vbCrLf)
        Thread.Sleep(1000)
        response = ReadDataWithRetry()
        LogTextbox.AppendText(response & vbCrLf)

        ' 執行 UIMA 命令
        LogTextbox.AppendText("執行 UIMA 命令..." & vbCrLf)
        WriteData("UIMA" & vbCrLf)
        Thread.Sleep(1000)
        response = ReadDataWithRetry()
        LogTextbox.AppendText(response & vbCrLf)

        ' 執行 UDAT 命令
        LogTextbox.AppendText("執行 UDAT 命令..." & vbCrLf)
        WriteData("UDAT" & vbCrLf)
        Thread.Sleep(1000)
        response = ReadDataWithRetry()
        LogTextbox.AppendText(response & vbCrLf)

        ' 執行 DAT 命令
        LogTextbox.AppendText("執行 DAT 命令..." & vbCrLf)
        WriteData("DAT" & vbCrLf)
        Thread.Sleep(1000)
        response = ReadDataWithRetry()
        LogTextbox.AppendText(response & vbCrLf)

        ' 確認是否成功設置環境
        If response.Contains("DTR>") Then
            LogTextbox.AppendText("環境初始化成功，已進入 DTR 模式" & vbCrLf)
        Else
            ' 再嘗試一次按Enter，確保進入正確模式
            WriteData(vbCrLf)
            Thread.Sleep(500)
            response = ReadDataWithRetry()
            LogTextbox.AppendText(response & vbCrLf)

            If response.Contains("DTR>") Then
                LogTextbox.AppendText("環境初始化成功，已進入 DTR 模式" & vbCrLf)
            Else
                LogTextbox.AppendText("警告: 未能確認 DTR 模式，但將繼續執行" & vbCrLf)
            End If
        End If
    End Sub

    ' 執行搜尋
    Private Sub PerformSearch()
        Try
            LogTextbox.AppendText("＊＊＊＊＊＊＊＊＊＊　搜尋中　＊＊＊＊＊＊＊＊＊＊" & vbCrLf)

            ' 確保連接器在正常狀態
            If T_Stream Is Nothing OrElse Not T_Stream.CanWrite Then
                LogTextbox.AppendText("連接未建立或已斷開，無法執行搜尋" & vbCrLf)
                Connected = False
                UpdateUIState(Connected)
                Return
            End If

            ' 確保命令提示符處於正確狀態
            LogTextbox.AppendText("檢查命令提示符狀態..." & vbCrLf)
            WriteData(vbCrLf)
            Thread.Sleep(500)
            Dim checkResponse = ReadDataWithRetry()
            LogTextbox.AppendText(checkResponse & vbCrLf)

            ' 如果沒看到DTR提示符，嘗試重新執行環境設置命令
            If Not checkResponse.TrimEnd().EndsWith("DTR>") Then
                LogTextbox.AppendText("未檢測到正確的命令提示符，嘗試重新初始化環境..." & vbCrLf)

                ' 執行 ULG 命令
                WriteData("ULG" & vbCrLf)
                Thread.Sleep(1000)
                checkResponse = ReadDataWithRetry()
                LogTextbox.AppendText(checkResponse & vbCrLf)

                ' 執行 UIMA 命令
                WriteData("UIMA" & vbCrLf)
                Thread.Sleep(1000)
                checkResponse = ReadDataWithRetry()
                LogTextbox.AppendText(checkResponse & vbCrLf)

                ' 執行 UDAT 命令
                WriteData("UDAT" & vbCrLf)
                Thread.Sleep(1000)
                checkResponse = ReadDataWithRetry()
                LogTextbox.AppendText(checkResponse & vbCrLf)

                ' 執行 DAT 命令
                WriteData("DAT" & vbCrLf)
                Thread.Sleep(1000)
                checkResponse = ReadDataWithRetry()
                LogTextbox.AppendText(checkResponse & vbCrLf)

                ' 確認是否成功進入DTR模式
                If Not checkResponse.Contains("DTR>") Then
                    LogTextbox.AppendText("警告: 可能未能進入DTR模式，但將嘗試繼續" & vbCrLf)
                End If
            End If

            ' 準備資料表 - 注意命令格式和間隔
            Dim tableName = Table_CB.Text.Replace("_", "-") ' 確保使用連字符而不是底線
            Dim readyCommand = "READY " & tableName & " SHARED WRITE"

            LogTextbox.AppendText("執行命令: " & readyCommand & vbCrLf)
            WriteData(readyCommand & vbCrLf)
            Thread.Sleep(1500) ' 增加等待時間

            Dim response = ReadDataWithRetry(3) ' 增加重試次數
            LogTextbox.AppendText(response & vbCrLf)

            ' 檢查資料表名稱是否有效
            If response.Contains("not found in dictionary") OrElse response.Contains("not a record name") Then
                LogTextbox.AppendText("＊＊＊＊＊　資料表名 " & tableName & " 錯誤　＊＊＊＊＊" & vbCrLf)
                LogTextbox.AppendText("嘗試使用替代格式重新執行..." & vbCrLf)

                ' 嘗試另一種格式 (去除連字符，讓系統自動解釋)
                tableName = Table_CB.Text.Replace("-", "")
                readyCommand = "READY " & tableName & " SHARED WRITE"

                LogTextbox.AppendText("執行命令: " & readyCommand & vbCrLf)
                WriteData(readyCommand & vbCrLf)
                Thread.Sleep(1500)

                response = ReadDataWithRetry(3)
                LogTextbox.AppendText(response & vbCrLf)

                If response.Contains("not found in dictionary") OrElse response.Contains("not a record name") Then
                    LogTextbox.AppendText("＊＊＊＊＊　資料表名仍然錯誤，無法繼續搜尋　＊＊＊＊＊" & vbCrLf)
                    Return
                End If
            End If

            ' 確保提示符出現後再進行下一步
            If Not response.TrimEnd().EndsWith("DTR>") Then
                Thread.Sleep(1000)
                Dim extraResponse = ReadDataWithRetry()
                If Not String.IsNullOrWhiteSpace(extraResponse) Then
                    LogTextbox.AppendText(extraResponse & vbCrLf)
                    response += extraResponse
                End If
            End If

            ' 取得資料表前綴用於搜尋 - 小心處理資料表名稱
            Dim tablePrefix As String
            If Table_CB.Text.Length >= 7 Then
                tablePrefix = Table_CB.Text.Substring(2, 5)
            Else
                tablePrefix = Table_CB.Text
                LogTextbox.AppendText("警告: 資料表名稱格式異常，使用完整名稱作為前綴" & vbCrLf)
            End If

            ' 執行搜尋命令 - 注意引號格式和空格
            Dim findCommand = "FIND " & tableName & " WITH " & tablePrefix & "-COIL-NO=""" & Coil_tbx.Text & """"

            LogTextbox.AppendText("執行命令: " & findCommand & vbCrLf)
            WriteData(findCommand & vbCrLf)
            Thread.Sleep(1500) ' 增加等待時間

            response = ReadDataWithRetry(3) ' 增加重試次數
            LogTextbox.AppendText(response & vbCrLf)

            ' 如果搜尋命令失敗，嘗試調整命令格式
            If response.Contains("not a readied source") OrElse response.Contains("Syntax error") Then
                LogTextbox.AppendText("搜尋命令格式錯誤，嘗試調整格式..." & vbCrLf)

                ' 嘗試替代的搜尋命令格式
                findCommand = "FIND " & tableName & " WITH " & tablePrefix.Replace("-", "") & "COILNO=""" & Coil_tbx.Text & """"

                LogTextbox.AppendText("執行替代命令: " & findCommand & vbCrLf)
                WriteData(findCommand & vbCrLf)
                Thread.Sleep(1500)

                response = ReadDataWithRetry(3)
                LogTextbox.AppendText(response & vbCrLf)
            End If

            ' 解析搜尋結果數量
            If response.Contains("found]") Then
                Dim pattern As String = "\[(\d+)\s+records? found\]"
                Dim Match1 As Match = Regex.Match(response, pattern)

                If Match1.Success Then
                    Dim match2 As Match = Regex.Match(Match1.Value, "\d+")

                    If match2.Success Then
                        SearchResults = Integer.Parse(match2.Value)
                        SearchRes_LB.Text = "搜尋結果：" & SearchResults & " 筆"

                        ' 啟用相關按鈕
                        lock_btn.Enabled = (SearchResults > 0)
                        LastSearchedCoil = Coil_tbx.Text

                        LogTextbox.AppendText("＊＊＊＊＊＊＊＊＊＊　搜尋完成，找到 " & SearchResults & " 筆資料　＊＊＊＊＊＊＊＊＊＊" & vbCrLf)
                    End If
                End If
            ElseIf response.Contains("No records found") Then
                SearchResults = 0
                SearchRes_LB.Text = "搜尋結果：0 筆"
                LogTextbox.AppendText("＊＊＊＊＊＊＊＊＊＊　搜尋完成，無符合條件的資料　＊＊＊＊＊＊＊＊＊＊" & vbCrLf)
            Else
                LogTextbox.AppendText("搜尋未返回有效結果，請檢查資料表格式或鋼捲編號" & vbCrLf)
            End If
        Catch ex As Exception
            LogTextbox.AppendText("搜尋出錯: " & ex.Message & vbCrLf)
        End Try

        ' 更新界面狀態
        UpdateUIState(Connected)
    End Sub

    ' 鎖定按鈕點擊事件
    Private Sub lock_btn_Click(sender As Object, e As EventArgs) Handles lock_btn.Click
        If Not Connected Then Return

        If String.IsNullOrEmpty(sel_tbx.Text) Then
            MessageBox.Show("請輸入要鎖定的資料筆數", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not IsNumeric(sel_tbx.Text) Then
            MessageBox.Show("鎖定筆數必須是數字", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim indexToLock As Integer = Integer.Parse(sel_tbx.Text)

        If indexToLock <= 0 OrElse indexToLock > SearchResults Then
            MessageBox.Show($"鎖定筆數超出範圍(1-{SearchResults})", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Cursor = Cursors.WaitCursor

            ' 執行鎖定命令
            Dim selectCommand = "SELECT " & indexToLock.ToString()
            WriteData(selectCommand & vbCrLf)
            Thread.Sleep(1000)
            Dim response = ReadDataWithRetry()
            LogTextbox.AppendText(response & vbCrLf)

            ' 列出資料
            WriteData("LIST" & vbCrLf)
            Thread.Sleep(1000)
            response = ReadDataWithRetry()
            LogTextbox.AppendText(response & vbCrLf)

            ' 解析並顯示資料
            ParseAndDisplayData(response)

            ' 設置鎖定的索引
            LastLockedIndex = indexToLock

            ' 初始化修改下拉框
            PopulateModifyCombobox()

            ' 更新界面狀態
            UpdateUIState(Connected)
        Catch ex As Exception
            LogTextbox.AppendText("鎖定出錯: " & ex.Message & vbCrLf)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    ' 解析並顯示資料
    Private Sub ParseAndDisplayData(data As String)
        ' 清空當前資料和表格
        currentDataRows.Clear()
        DataGridView1.Rows.Clear()

        ' 分割每行資料
        Dim lines() As String = data.Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)

        ' 逐行處理資料
        For Each line In lines
            ' 忽略命令行和提示符
            If line.StartsWith("SELECT") OrElse line.StartsWith("LIST") OrElse line.StartsWith("DTR>") Then
                Continue For
            End If

            ' 將每行按 " : " 分割成鍵和值
            Dim parts() As String = line.Split(New String() {" : "}, StringSplitOptions.None)

            ' 確保有兩個部分
            If parts.Length = 2 Then
                Dim field = parts(0).Trim()
                Dim value = parts(1).Trim()

                ' 添加到資料集合
                currentDataRows.Add(New String() {field, value})

                ' 添加到表格
                DataGridView1.Rows.Add(DataGridView1.Rows.Count + 1, field, value)
            End If
        Next
    End Sub

    ' 填充修改下拉框
    Private Sub PopulateModifyCombobox()
        Modify_cb.Items.Clear()

        ' 自訂選項 - 使用指定格式 "Modify 欄位名"
        Modify_cb.Items.Add("M6515-ORDER-ITEM")
        Modify_cb.Items.Add("M6515-ORDER-GAUGE")
        Modify_cb.Items.Add("M6515-ACT-GAUGE")
        Modify_cb.Items.Add("M6515-ORDER-WIDTH")
        Modify_cb.Items.Add("M6515-ACT-WIDTH")
        Modify_cb.Items.Add("M6515-MIC-NO")
        Modify_cb.Items.Add("M6515-PRODUCT")

        ' 如果有項目則選擇第一項
        If Modify_cb.Items.Count > 0 Then
            Modify_cb.SelectedIndex = 0
        End If
    End Sub

    ' 修改欄位選擇變更事件
    Private Sub Modify_cb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Modify_cb.SelectedIndexChanged
        Try
            ' 首先確保有選擇項目
            If Modify_cb.SelectedIndex < 0 Then
                Modify_tbx.Text = ""
                Modify_tbx.Enabled = False
                Return
            End If

            ' 清空並啟用輸入框，讓用戶自行輸入
            Modify_tbx.Text = ""
            Modify_tbx.Enabled = True

            ' 記錄選擇的項目（僅供Log）
            Dim selectedItem As String = Modify_cb.SelectedItem.ToString()
            LogTextbox.AppendText("選擇項: " & selectedItem & vbCrLf)

            ' 取得欄位名稱（僅供Log）
            Dim parts As String() = selectedItem.Split("|"c)
            If parts.Length >= 2 Then
                Dim fieldName As String = parts(1).Trim()
                LogTextbox.AppendText("欄位名稱: " & fieldName & vbCrLf)
            End If
        Catch ex As Exception
            LogTextbox.AppendText("選擇欄位時發生錯誤: " & ex.Message & vbCrLf)
            Modify_tbx.Text = ""
        End Try

        ' 更新界面狀態
        UpdateUIState(Connected)
    End Sub

    ' LockModify_btn 點擊事件 (原 Recover_btn_Click)
    Private Sub LockModify_btn_Click(sender As Object, e As EventArgs) Handles LockModify_btn.Click
        If Not Connected Or DataGridView1.Rows.Count = 0 Then Return
        If Modify_cb.SelectedIndex < 0 Then
            MessageBox.Show("請選擇要修改的欄位", "選擇錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Try
            Cursor = Cursors.WaitCursor
            ' 送出 MODIFY 指令
            Dim selectedItem As String = Modify_cb.SelectedItem.ToString()
            Dim parts As String() = selectedItem.Split("|"c)
            Dim fieldName As String = If(parts.Length >= 2, parts(1).Trim(), selectedItem.Trim())
            Dim modifyCommand = "MODIFY " & fieldName
            LogTextbox.AppendText("執行命令: " & modifyCommand & vbCrLf)
            WriteData(modifyCommand & vbCrLf)
            Thread.Sleep(1200)
            Dim response = ReadDataWithRetry(3)
            LogTextbox.AppendText(response & vbCrLf)
            ' 進入鎖定狀態，啟用輸入框與確認按鈕
            IsModifyLocked = True
            Modify_tbx.Text = ""
            UpdateUIState(Connected)
        Catch ex As Exception
            LogTextbox.AppendText("LockModify出錯: " & ex.Message & vbCrLf)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    ' 確認修改按鈕點擊事件
    Private Sub Confirm_btn_Click(sender As Object, e As EventArgs) Handles Confirm_btn.Click
        ' 檢查基本條件
        If Not Connected Then
            LogTextbox.AppendText("錯誤: 未連接到服務器" & vbCrLf)
            Return
        End If

        If LastLockedIndex < 0 Then
            LogTextbox.AppendText("錯誤: 未鎖定任何數據" & vbCrLf)
            Return
        End If

        If String.IsNullOrEmpty(Modify_cb.Text) Then
            MessageBox.Show("請選擇要修改的欄位", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrEmpty(Modify_tbx.Text) Then
            MessageBox.Show("請輸入修改的值", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            ' 設置界面
            Cursor = Cursors.WaitCursor
            ModifyMode = True

            ' 送出新值
            LogTextbox.AppendText("輸入新值: " & Modify_tbx.Text & vbCrLf)
            WriteData(Modify_tbx.Text & vbCrLf)
            Thread.Sleep(1500)
            Dim response = ReadDataWithRetry(3)
            LogTextbox.AppendText(response & vbCrLf)

            ' 更新資料顯示
            LogTextbox.AppendText("更新資料顯示..." & vbCrLf)
            WriteData("LIST" & vbCrLf)
            Thread.Sleep(2000)
            response = ReadDataWithRetry(4)
            LogTextbox.AppendText(response & vbCrLf)
            ParseAndDisplayData(response)

            ' 完成修改
            ModifyMode = False
            IsModifyLocked = False ' 重置狀態
            LogTextbox.AppendText("＊＊＊＊＊＊＊＊＊＊　修改完成　＊＊＊＊＊＊＊＊＊＊" & vbCrLf)

            ' 清空輸入框
            Modify_tbx.Text = ""

            ' 更新界面狀態
            UpdateUIState(Connected)
        Catch ex As Exception
            LogTextbox.AppendText("修改出錯: " & ex.Message & vbCrLf)
            ModifyMode = False
            IsModifyLocked = False
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    ' 取消修改按鈕點擊事件
    Private Sub Cancel_btn_Click(sender As Object, e As EventArgs) Handles Cancel_btn.Click
        Try
            LogTextbox.AppendText("取消修改操作" & vbCrLf)

            ' 重置修改狀態
            ModifyMode = False
            Dim needList As Boolean = False

            If IsModifyLocked Then
                ' 若已進入修改鎖定，送出 Ctrl+Z 放棄修改
                LogTextbox.AppendText("送出 Ctrl+Z 放棄修改..." & vbCrLf)
                WriteData(Chr(26)) ' Ctrl+Z
                Thread.Sleep(500)
                needList = True
                IsModifyLocked = False
            End If

            ' 如果需要從服務器重新獲取當前數據
            If (Connected And LastLockedIndex >= 0) Or needList Then
                LogTextbox.AppendText("重新獲取當前數據..." & vbCrLf)

                ' 禁用控件先
                Modify_cb.Enabled = False
                Modify_tbx.Enabled = False
                Cancel_btn.Enabled = False
                Confirm_btn.Enabled = False

                WriteData("LIST" & vbCrLf)
                Thread.Sleep(1000)
                Dim response = ReadDataWithRetry(3)
                LogTextbox.AppendText(response & vbCrLf)

                ' 解析並顯示原始資料
                ParseAndDisplayData(response)

                ' 重新填充修改下拉框，確保正確的欄位顯示
                PopulateModifyCombobox()
            Else
                ' 未連接或未鎖定任何記錄，清空下拉框和文本框
                Modify_cb.Text = ""
                Modify_tbx.Text = ""
            End If

            ' 更新界面狀態 - 確保下拉框在有記錄被鎖定時是啟用的
            UpdateUIState(Connected)

            ' 主動選擇下拉框的第一項
            If Modify_cb.Items.Count > 0 Then
                Modify_cb.SelectedIndex = 0
            End If

        Catch ex As Exception
            LogTextbox.AppendText("取消操作出錯: " & ex.Message & vbCrLf)
        End Try
    End Sub

    ' 讀取資料
    Private Function ReadData() As String
        Dim sData As String = ""

        Try
            If T_Stream IsNot Nothing AndAlso T_Stream.CanRead Then
                Dim buffer(T_Client.ReceiveBufferSize - 1) As Byte
                Dim bytesRead As Integer = 0

                ' 等待資料可讀
                Dim startTime = DateTime.Now
                While Not T_Stream.DataAvailable AndAlso DateTime.Now.Subtract(startTime).TotalSeconds < 5 ' 增加了等待時間 
                    Thread.Sleep(100)
                End While

                ' 如果有資料可讀，則讀取
                If T_Stream.DataAvailable Then
                    bytesRead = T_Stream.Read(buffer, 0, buffer.Length)

                    ' 嘗試用 BIG5 解碼，如果失敗則用默認編碼
                    Try
                        sData = big5Encoding.GetString(buffer, 0, bytesRead)
                    Catch
                        sData = Encoding.Default.GetString(buffer, 0, bytesRead)
                    End Try

                    ' 再等待一下，確保所有數據都已讀完
                    Thread.Sleep(300)

                    ' 如果還有更多數據，繼續讀取
                    While T_Stream.DataAvailable
                        bytesRead = T_Stream.Read(buffer, 0, buffer.Length)
                        Try
                            sData += big5Encoding.GetString(buffer, 0, bytesRead)
                        Catch
                            sData += Encoding.Default.GetString(buffer, 0, bytesRead)
                        End Try
                        Thread.Sleep(100)
                    End While
                End If
            End If
        Catch ex As Exception
            LogTextbox.AppendText("讀取資料出錯: " & ex.Message & vbCrLf)
        End Try

        Return sData.Trim()
    End Function

    ' 寫入資料
    Private Sub WriteData(ByVal sData As String)
        Try
            If T_Stream IsNot Nothing AndAlso T_Stream.CanWrite Then
                bytWriting = big5Encoding.GetBytes(sData)
                T_Stream.Write(bytWriting, 0, bytWriting.Length)
                T_Stream.Flush()
            End If
        Catch ex As Exception
            LogTextbox.AppendText("寫入資料出錯: " & ex.Message & vbCrLf)
        End Try
    End Sub

    ' 關閉連接
    Private Sub CloseConnection()
        Try
            If T_Stream IsNot Nothing Then
                T_Stream.Close()
                T_Stream = Nothing
            End If

            If T_Client IsNot Nothing Then
                T_Client.Close()
                T_Client = New TcpClient()
            End If

            Connected = False
            LastLockedIndex = -1
            ModifyMode = False
            IsModifyLocked = False

            ' 更新界面狀態
            UpdateUIState(Connected)
        Catch ex As Exception
            LogTextbox.AppendText("關閉連接出錯: " & ex.Message & vbCrLf)
        End Try
    End Sub

    ' 表單關閉時關閉連接
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseConnection()
    End Sub

    ' 登出按鈕點擊事件
    Private Sub logout_btn_Click(sender As Object, e As EventArgs) Handles Logout_btn.Click
        Try
            LogTextbox.AppendText("執行登出..." & vbCrLf)
            If Connected AndAlso T_Stream IsNot Nothing AndAlso T_Stream.CanWrite Then
                WriteData("EXIT" & vbCrLf)
                Thread.Sleep(500)
                WriteData("lo" & vbCrLf)
                Thread.Sleep(500)
            End If
            CloseConnection()
            LogTextbox.AppendText("已登出並關閉連線。" & vbCrLf)
        Catch ex As Exception
            LogTextbox.AppendText("登出出錯: " & ex.Message & vbCrLf)
        End Try
        UpdateUIState(False)
    End Sub
End Class

Module Program
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New Form1())
    End Sub
End Module
