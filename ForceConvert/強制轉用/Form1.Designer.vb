<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Acc_LB = New System.Windows.Forms.Label()
        Me.TableSelect_LB = New System.Windows.Forms.Label()
        Me.CoilNumber_LB = New System.Windows.Forms.Label()
        Me.SearchRes_LB = New System.Windows.Forms.Label()
        Me.ACCSetting_LB = New System.Windows.Forms.Label()
        Me.sel_tbx = New System.Windows.Forms.TextBox()
        Me.Acc_TB = New System.Windows.Forms.TextBox()
        Me.Coil_tbx = New System.Windows.Forms.TextBox()
        Me.Table_CB = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Modify_cb = New System.Windows.Forms.ComboBox()
        Me.Modify_tbx = New System.Windows.Forms.TextBox()
        Me.Login_btn = New System.Windows.Forms.Button()
        Me.lock_btn = New System.Windows.Forms.Button()
        Me.LockModify_btn = New System.Windows.Forms.Button()
        Me.Confirm_btn = New System.Windows.Forms.Button()
        Me.Cancel_btn = New System.Windows.Forms.Button()
        Me.Logout_btn = New System.Windows.Forms.Button()
        Me.LogTextbox = New System.Windows.Forms.TextBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Acc_LB
        '
        Me.Acc_LB.AutoSize = True
        Me.Acc_LB.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Acc_LB.Location = New System.Drawing.Point(26, 66)
        Me.Acc_LB.Name = "Acc_LB"
        Me.Acc_LB.Size = New System.Drawing.Size(54, 26)
        Me.Acc_LB.TabIndex = 0
        Me.Acc_LB.Text = "帳號"
        '
        'TableSelect_LB
        '
        Me.TableSelect_LB.AutoSize = True
        Me.TableSelect_LB.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.TableSelect_LB.Location = New System.Drawing.Point(26, 114)
        Me.TableSelect_LB.Name = "TableSelect_LB"
        Me.TableSelect_LB.Size = New System.Drawing.Size(75, 26)
        Me.TableSelect_LB.TabIndex = 1
        Me.TableSelect_LB.Text = "資料表"
        '
        'CoilNumber_LB
        '
        Me.CoilNumber_LB.AutoSize = True
        Me.CoilNumber_LB.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.CoilNumber_LB.Location = New System.Drawing.Point(26, 163)
        Me.CoilNumber_LB.Name = "CoilNumber_LB"
        Me.CoilNumber_LB.Size = New System.Drawing.Size(96, 26)
        Me.CoilNumber_LB.TabIndex = 2
        Me.CoilNumber_LB.Text = "鋼捲編號"
        '
        'SearchRes_LB
        '
        Me.SearchRes_LB.AutoSize = True
        Me.SearchRes_LB.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.SearchRes_LB.Location = New System.Drawing.Point(103, 296)
        Me.SearchRes_LB.Name = "SearchRes_LB"
        Me.SearchRes_LB.Size = New System.Drawing.Size(147, 26)
        Me.SearchRes_LB.TabIndex = 3
        Me.SearchRes_LB.Text = "搜尋結果 : ? 筆"
        '
        'ACCSetting_LB
        '
        Me.ACCSetting_LB.AutoSize = True
        Me.ACCSetting_LB.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.ACCSetting_LB.Location = New System.Drawing.Point(126, 16)
        Me.ACCSetting_LB.Name = "ACCSetting_LB"
        Me.ACCSetting_LB.Size = New System.Drawing.Size(96, 26)
        Me.ACCSetting_LB.TabIndex = 4
        Me.ACCSetting_LB.Text = "設定狀況"
        '
        'sel_tbx
        '
        Me.sel_tbx.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.sel_tbx.Location = New System.Drawing.Point(67, 354)
        Me.sel_tbx.Name = "sel_tbx"
        Me.sel_tbx.Size = New System.Drawing.Size(55, 35)
        Me.sel_tbx.TabIndex = 5
        Me.sel_tbx.Text = "1"
        Me.sel_tbx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Acc_TB
        '
        Me.Acc_TB.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Acc_TB.Location = New System.Drawing.Point(122, 63)
        Me.Acc_TB.Name = "Acc_TB"
        Me.Acc_TB.ReadOnly = True
        Me.Acc_TB.Size = New System.Drawing.Size(210, 35)
        Me.Acc_TB.TabIndex = 6
        Me.Acc_TB.Text = "T16510"
        Me.Acc_TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Coil_tbx
        '
        Me.Coil_tbx.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Coil_tbx.Location = New System.Drawing.Point(122, 160)
        Me.Coil_tbx.Name = "Coil_tbx"
        Me.Coil_tbx.Size = New System.Drawing.Size(210, 35)
        Me.Coil_tbx.TabIndex = 7
        Me.Coil_tbx.Text = "42190291"
        Me.Coil_tbx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Table_CB
        '
        Me.Table_CB.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Table_CB.FormattingEnabled = True
        Me.Table_CB.Location = New System.Drawing.Point(122, 111)
        Me.Table_CB.Name = "Table_CB"
        Me.Table_CB.Size = New System.Drawing.Size(210, 34)
        Me.Table_CB.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(680, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 26)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "搜尋紀錄"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label2.Location = New System.Drawing.Point(126, 358)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 26)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "筆"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label3.Location = New System.Drawing.Point(26, 358)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 26)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "第"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label4.Location = New System.Drawing.Point(680, 385)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 26)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "資料顯示"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label5.Location = New System.Drawing.Point(26, 408)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 26)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "修改"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label6.Location = New System.Drawing.Point(26, 531)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(33, 26)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "值"
        '
        'Modify_cb
        '
        Me.Modify_cb.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Modify_cb.FormattingEnabled = True
        Me.Modify_cb.Location = New System.Drawing.Point(86, 405)
        Me.Modify_cb.Name = "Modify_cb"
        Me.Modify_cb.Size = New System.Drawing.Size(239, 34)
        Me.Modify_cb.TabIndex = 15
        '
        'Modify_tbx
        '
        Me.Modify_tbx.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Modify_tbx.Location = New System.Drawing.Point(86, 528)
        Me.Modify_tbx.Name = "Modify_tbx"
        Me.Modify_tbx.Size = New System.Drawing.Size(239, 35)
        Me.Modify_tbx.TabIndex = 16
        '
        'Login_btn
        '
        Me.Login_btn.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Login_btn.Location = New System.Drawing.Point(43, 222)
        Me.Login_btn.Name = "Login_btn"
        Me.Login_btn.Size = New System.Drawing.Size(289, 40)
        Me.Login_btn.TabIndex = 17
        Me.Login_btn.Text = "第一次搜尋及登入"
        Me.Login_btn.UseVisualStyleBackColor = True
        '
        'lock_btn
        '
        Me.lock_btn.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lock_btn.Location = New System.Drawing.Point(162, 351)
        Me.lock_btn.Name = "lock_btn"
        Me.lock_btn.Size = New System.Drawing.Size(163, 40)
        Me.lock_btn.TabIndex = 17
        Me.lock_btn.Text = "鎖定並顯示"
        Me.lock_btn.UseVisualStyleBackColor = True
        '
        'LockModify_btn
        '
        Me.LockModify_btn.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.LockModify_btn.Location = New System.Drawing.Point(31, 453)
        Me.LockModify_btn.Name = "LockModify_btn"
        Me.LockModify_btn.Size = New System.Drawing.Size(294, 50)
        Me.LockModify_btn.TabIndex = 17
        Me.LockModify_btn.Text = "選定修改資料行"
        Me.LockModify_btn.UseVisualStyleBackColor = True
        '
        'Confirm_btn
        '
        Me.Confirm_btn.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Confirm_btn.Location = New System.Drawing.Point(31, 590)
        Me.Confirm_btn.Name = "Confirm_btn"
        Me.Confirm_btn.Size = New System.Drawing.Size(144, 50)
        Me.Confirm_btn.TabIndex = 17
        Me.Confirm_btn.Text = "確認修改"
        Me.Confirm_btn.UseVisualStyleBackColor = True
        '
        'Cancel_btn
        '
        Me.Cancel_btn.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Cancel_btn.Location = New System.Drawing.Point(181, 590)
        Me.Cancel_btn.Name = "Cancel_btn"
        Me.Cancel_btn.Size = New System.Drawing.Size(144, 50)
        Me.Cancel_btn.TabIndex = 17
        Me.Cancel_btn.Text = "取消"
        Me.Cancel_btn.UseVisualStyleBackColor = True
        '
        'Logout_btn
        '
        Me.Logout_btn.Font = New System.Drawing.Font("微軟正黑體", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Logout_btn.Location = New System.Drawing.Point(31, 657)
        Me.Logout_btn.Name = "Logout_btn"
        Me.Logout_btn.Size = New System.Drawing.Size(294, 40)
        Me.Logout_btn.TabIndex = 17
        Me.Logout_btn.Text = "登出"
        Me.Logout_btn.UseVisualStyleBackColor = True
        '
        'LogTextbox
        '
        Me.LogTextbox.Font = New System.Drawing.Font("微軟正黑體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.LogTextbox.Location = New System.Drawing.Point(357, 52)
        Me.LogTextbox.Multiline = True
        Me.LogTextbox.Name = "LogTextbox"
        Me.LogTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LogTextbox.Size = New System.Drawing.Size(708, 314)
        Me.LogTextbox.TabIndex = 18
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(357, 423)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(708, 299)
        Me.DataGridView1.TabIndex = 19
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1077, 729)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.LogTextbox)
        Me.Controls.Add(Me.Logout_btn)
        Me.Controls.Add(Me.Cancel_btn)
        Me.Controls.Add(Me.Confirm_btn)
        Me.Controls.Add(Me.LockModify_btn)
        Me.Controls.Add(Me.lock_btn)
        Me.Controls.Add(Me.Login_btn)
        Me.Controls.Add(Me.Modify_tbx)
        Me.Controls.Add(Me.Modify_cb)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Table_CB)
        Me.Controls.Add(Me.Coil_tbx)
        Me.Controls.Add(Me.Acc_TB)
        Me.Controls.Add(Me.sel_tbx)
        Me.Controls.Add(Me.ACCSetting_LB)
        Me.Controls.Add(Me.SearchRes_LB)
        Me.Controls.Add(Me.CoilNumber_LB)
        Me.Controls.Add(Me.TableSelect_LB)
        Me.Controls.Add(Me.Acc_LB)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Acc_LB As Label
    Friend WithEvents TableSelect_LB As Label
    Friend WithEvents CoilNumber_LB As Label
    Friend WithEvents SearchRes_LB As Label
    Friend WithEvents ACCSetting_LB As Label
    Friend WithEvents sel_tbx As TextBox
    Friend WithEvents Acc_TB As TextBox
    Friend WithEvents Coil_tbx As TextBox
    Friend WithEvents Table_CB As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Modify_cb As ComboBox
    Friend WithEvents Modify_tbx As TextBox
    Friend WithEvents Login_btn As Button
    Friend WithEvents lock_btn As Button
    Friend WithEvents LockModify_btn As Button
    Friend WithEvents Confirm_btn As Button
    Friend WithEvents Cancel_btn As Button
    Friend WithEvents Logout_btn As Button
    Friend WithEvents LogTextbox As TextBox
    Friend WithEvents DataGridView1 As DataGridView
End Class
