﻿Public Class Form1
    Private Const Zero = "0"
    Private _buffer As Decimal = 0
    Private _op As String = Nothing
    Private _clear As Boolean = True
    Private ReadOnly _acceptedChars As Dictionary(Of Char, Boolean) = "0123456789.".ToDictionary(Function(k) k, Function(c) True)
    Private ReadOnly _acceptedOperators As Dictionary(Of Char, Boolean) = "+-=*/".ToDictionary(Function(k) k, Function(c) True)

    Private Sub NumberButtonClick(sender As Object, e As EventArgs) Handles Number1Btn.Click, Number2Btn.Click, Number3Btn.Click, Number4Btn.Click, Number5Btn.Click, Number6Btn.Click, Number7Btn.Click, Number8Btn.Click, Number9Btn.Click, Number0Btn.Click, DecimalBtn.Click
        AppendTextToNumber(GetTextOfControl(sender).Trim().First())
    End Sub

    Private Sub CeClickBtn(sender As Object, e As EventArgs) Handles CeBtn.Click
        DisplayLbl.Text = Zero
        _clear = True
    End Sub

    Private Sub CClickBtn(sender As Object, e As EventArgs) Handles CBtn.Click
        _buffer = 0
        _op = Nothing
        CeClickBtn(sender, e)
    End Sub

    Private Sub OperatorClick(sender As Object, e As EventArgs) Handles OperatorPlusBtn.Click, OperatorSubtractBtn.Click, OperatorDivideBtn.Click, OperatorMultiplyBtn.Click
        Dim txt As String = GetTextOfControl(sender)
        OperatorHandle(txt, sender, e)
    End Sub

    Private Sub EqualClick(sender As Object, e As EventArgs) Handles EqualBtn.Click
        _buffer = Calc(_op, _buffer, DisplayLbl.Text)
        If _buffer < Integer.MaxValue Then
            If Decimal.ToInt32(_buffer) = _buffer Then
                _buffer = Decimal.ToInt32(_buffer)
            End If
        End If

        DisplayLbl.Text = _buffer
        _clear = True
    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        AppendTextToNumber(e.KeyChar)
        OperatorHandle(e.KeyChar, sender, e)
    End Sub

    Private Sub AppendTextToNumber(ByVal txt As Char)
        If _acceptedChars.ContainsKey(txt) = False Then
            Return
        End If
        If _clear Then
            DisplayLbl.Text = ""
        End If
        If txt = "." And DisplayLbl.Text.Contains(".") Then
            Return
        End If
        DisplayLbl.Text += txt
        _clear = False
    End Sub

    Private Sub OperatorHandle(ByVal txt As Char, sender As Object, e As EventArgs)
        If _acceptedOperators.ContainsKey(txt) = False Then
            Return
        End If
        If _op Is Nothing Or _clear Then
            _op = txt
            If _clear = False Then
                _buffer = Decimal.Parse(DisplayLbl.Text)
            End If
            _clear = True
            Return
        End If

        EqualClick(sender, e)
        _op = txt
    End Sub

    Private Shared Function Calc(ByVal op As String, ByVal buffer As Decimal?, ByVal labelText As String) As Decimal
        Dim labelNum As Decimal = Decimal.Parse(labelText)
        If op = "+" Then
            buffer += labelNum
        ElseIf op = "-" Then
            buffer -= labelNum
        ElseIf op = "*" Then
            buffer *= labelNum
        ElseIf op = "/" And labelNum <> 0 Then
            buffer /= labelNum
        End If
        Return buffer
    End Function

    Private Shared Function GetTextOfControl(sender As Object) As String
        Return CType(sender, Control).Text
    End Function

End Class


'' Over kill but more efficient. if you also removed the dictionary checks.
'' Decided against it as need the checks imo as a function should check values
'Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
'    Select Case e.KeyCode
'        Case Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5 _
'            , Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9
'            Dim num As Integer = e.KeyCode - Keys.NumPad0
'            AppendTextToNumber(num.ToString().First())

'        Case Keys.Decimal
'            AppendTextToNumber("."c)
'        Case Keys.Add
'            OperatorHandle("+")
'        Case Keys.Subtract
'            OperatorHandle("-")
'        Case Keys.Multiply
'            OperatorHandle("*")
'        Case Keys.Divide
'            OperatorHandle("/")
'        Case Keys.Oemplus
'            OperatorHandle("=")
'    End Select
'End Sub