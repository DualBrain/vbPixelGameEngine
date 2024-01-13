Option Explicit On
Option Strict On
Option Infer On

Namespace Global.QB

  Partial Public NotInheritable Class Core

    Private Sub New()
    End Sub

    ' Variables and Types

#Region "SWAP"

    Public Shared Sub SWAP(ByRef value1 As Short, ByRef value2 As Short)
      Dim temp = value1
      value1 = value2
      value2 = temp
    End Sub

    Public Shared Sub SWAP(ByRef value1 As String, ByRef value2 As String)
      Dim temp = value1
      value1 = value2
      value2 = temp
    End Sub

    Public Shared Sub SWAP(ByRef value1 As Single, ByRef value2 As Single)
      Dim temp = value1
      value1 = value2
      value2 = temp
    End Sub

    Public Shared Sub SWAP(ByRef value1 As Integer, ByRef value2 As Integer)
      Dim temp = value1
      value1 = value2
      value2 = temp
    End Sub

#End Region


    ' Flow Control

    ' Decisions and Operators

    ' Procedures

    ' Strings

    Public Shared Function QBAsc%(value$)

      Select Case value$

        Case vbNullChar : Return 0
        Case "☺" : Return 1
        Case "☻" : Return 2
        Case "♥" : Return 3
        Case "♦" : Return 4
        Case "♣" : Return 5
        Case "♠" : Return 6
        Case "●" : Return 7
        Case "◘" : Return 8
        Case "○" : Return 9
        Case "◙" : Return 10
        Case "♂" : Return 11
        Case "♀" : Return 12
        Case "♪" : Return 13
        Case "♫" : Return 14
        Case "☼" : Return 15
        Case "►" : Return 16
        Case "◄" : Return 17
        Case "↕" : Return 18
        Case "‼" : Return 19
        Case "¶" : Return 20
        Case "§" : Return 21
        Case "▬" : Return 22
        Case "↨" : Return 23
        Case "↑" : Return 24
        Case "↓" : Return 25

        Case "→" : Return 26
        Case "←" : Return 27
        Case "∟" : Return 28
        Case "↔" : Return 29

        Case "▲" : Return 30
        Case "▼" : Return 31

        Case " " : Return 32

        Case "∆" : Return 33

'http://ascii-table.com/ascii-extended-pc-list.php

        Case "Ç" : Return 128
        Case "ü" : Return 129
        Case "é" : Return 130
        Case "â" : Return 131
        Case "ä" : Return 132
        Case "à" : Return 133
        Case "å" : Return 134
        Case "ç" : Return 135
        Case "ê" : Return 136
        Case "ë" : Return 137
        Case "è" : Return 138
        Case "ï" : Return 139
        Case "î" : Return 140
        Case "ì" : Return 141
        Case "Ä" : Return 142
        Case "Å" : Return 143
        Case "É" : Return 144
        Case "æ" : Return 145
        Case "Æ" : Return 146
        Case "ô" : Return 147
        Case "ö" : Return 148
        Case "ò" : Return 149
        Case "û" : Return 150
        Case "ù" : Return 151
        Case "ÿ" : Return 152
        Case "Ö" : Return 153
        Case "Ü" : Return 154
        Case "¢" : Return 155
        Case "£" : Return 156
        Case "¥" : Return 157
        Case "₧" : Return 158
        Case "ƒ" : Return 159
        Case "á" : Return 160
        Case "í" : Return 161
        Case "ó" : Return 162
        Case "ú" : Return 163
        Case "ñ" : Return 164
        Case "Ñ" : Return 165
        Case "ª" : Return 166
        Case "º" : Return 167
        Case "¿" : Return 168
        Case "⌐" : Return 169
        Case "¬" : Return 170
        Case "½" : Return 171
        Case "¼" : Return 172
        Case "¡" : Return 173
        Case "«" : Return 174
        Case "»" : Return 175
        Case "░" : Return 176
        Case "▒" : Return 177
        Case "▓" : Return 178
        Case "│" : Return 179
        Case "┤" : Return 180
        Case "╡" : Return 181
        Case "╢" : Return 182
        Case "╖" : Return 183
        Case "╕" : Return 184
        Case "╣" : Return 185
        Case "║" : Return 186
        Case "╗" : Return 187
        Case "╝" : Return 188
        Case "╜" : Return 189
        Case "╛" : Return 190
        Case "┐" : Return 191
        Case "└" : Return 192
        Case "┴" : Return 193
        Case "┬" : Return 194
        Case "├" : Return 195
        Case "─" : Return 196
        Case "┼" : Return 197
        Case "╞" : Return 198
        Case "╟" : Return 199
        Case "╚" : Return 200
        Case "╔" : Return 201
        Case "╩" : Return 202
        Case "╦" : Return 203
        Case "╠" : Return 204
        Case "═" : Return 205
        Case "╬" : Return 206
        Case "╧" : Return 207
        Case "╨" : Return 208
        Case "╤" : Return 209
        Case "╥" : Return 210
        Case "╙" : Return 211
        Case "╘" : Return 212
        Case "╒" : Return 213
        Case "╓" : Return 214
        Case "╫" : Return 215
        Case "╪" : Return 216
        Case "┘" : Return 217
        Case "┌" : Return 218
        Case "█" : Return 219
        Case "▄" : Return 220
        Case "▌" : Return 221
        Case "▐" : Return 222
        Case "▀" : Return 223
        Case "α" : Return 224
        Case "ß" : Return 225
        Case "Γ" : Return 226
        Case "π" : Return 227
        Case "Σ" : Return 228
        Case "σ" : Return 229
        Case "µ" : Return 230
        Case "τ" : Return 231
        Case "Φ" : Return 232
        Case "Θ" : Return 233
        Case "Ω" : Return 234
        Case "δ" : Return 235
        Case "∞" : Return 236
        Case "φ" : Return 237
        Case "ε" : Return 238
        Case "∩" : Return 239
        Case "≡" : Return 240
        Case "±" : Return 241
        Case "≥" : Return 242
        Case "≤" : Return 243
        Case "⌠" : Return 244
        Case "⌡" : Return 245
        Case "÷" : Return 246
        Case "≈" : Return 247
        Case "°" : Return 248
        Case "●" : Return 249
        Case "·" : Return 250
        Case "√" : Return 251
        Case "ⁿ" : Return 252
        Case "²" : Return 253
        Case "■" : Return 254
        Case " " : Return 255

        Case "�" : Return 32

        Case Else
          Return Asc(value)
      End Select

    End Function

    Public Shared Function QBChr$(value%)

      '┘ └ ┌ ┐ ─ │ ├ ├ ╟ ╙ ╓ ╥ ╨ ┬ ┴ ║ ┤

      'Return ChrW(value%)

      Select Case value%

        Case 0 : Return vbNullChar ' " "
        Case 1 : Return "☺"
        Case 2 : Return "☻"
        Case 3 : Return "♥"
        Case 4 : Return "♦"
        Case 5 : Return "♣"
        Case 6 : Return "♠"
        Case 7 : Return "●"
        Case 8 : Return "◘"
        Case 9 : Return "○"
        Case 10 : Return "◙"
        Case 11 : Return "♂"
        Case 12 : Return "♀"
        Case 13 : Return "♪"
        Case 14 : Return "♫"
        Case 15 : Return "☼"
        Case 16 : Return "►"
        Case 17 : Return "◄"
        Case 18 : Return "↕"
        Case 19 : Return "‼"
        Case 20 : Return "¶"
        Case 21 : Return "§"
        Case 22 : Return "▬"
        Case 23 : Return "↨"
        Case 24 : Return "↑" ' ?
        Case 25 : Return "↓" ' ?

        Case 26 : Return "→"
        Case 27 : Return "←"
        Case 28 : Return "∟"
        Case 29 : Return "↔"

        Case 30 : Return "▲" '?
        Case 31 : Return "▼" '?

        Case 32 : Return " "

        Case 127 : Return "∆"

'http://ascii-table.com/ascii-extended-pc-list.php

        Case 128 : Return "Ç"
        Case 129 : Return "ü"
        Case 130 : Return "é"
        Case 131 : Return "â"
        Case 132 : Return "ä"
        Case 133 : Return "à"
        Case 134 : Return "å"
        Case 135 : Return "ç"
        Case 136 : Return "ê"
        Case 137 : Return "ë"
        Case 138 : Return "è"
        Case 139 : Return "ï"
        Case 140 : Return "î"
        Case 141 : Return "ì"
        Case 142 : Return "Ä"
        Case 143 : Return "Å"
        Case 144 : Return "É"
        Case 145 : Return "æ"
        Case 146 : Return "Æ"
        Case 147 : Return "ô"
        Case 148 : Return "ö"
        Case 149 : Return "ò"
        Case 150 : Return "û"
        Case 151 : Return "ù"
        Case 152 : Return "ÿ"
        Case 153 : Return "Ö"
        Case 154 : Return "Ü"
        Case 155 : Return "¢"
        Case 156 : Return "£"
        Case 157 : Return "¥"
        Case 158 : Return "₧"
        Case 159 : Return "ƒ"
        Case 160 : Return "á"
        Case 161 : Return "í"
        Case 162 : Return "ó"
        Case 163 : Return "ú"
        Case 164 : Return "ñ"
        Case 165 : Return "Ñ"
        Case 166 : Return "ª"
        Case 167 : Return "º"
        Case 168 : Return "¿"
        Case 169 : Return "⌐"
        Case 170 : Return "¬"
        Case 171 : Return "½"
        Case 172 : Return "¼"
        Case 173 : Return "¡"
        Case 174 : Return "«"
        Case 175 : Return "»"
        Case 176 : Return "░"
        Case 177 : Return "▒"
        Case 178 : Return "▓"
        Case 179 : Return "│"
        Case 180 : Return "┤"
        Case 181 : Return "╡"
        Case 182 : Return "╢"
        Case 183 : Return "╖"
        Case 184 : Return "╕"
        Case 185 : Return "╣"
        Case 186 : Return "║"
        Case 187 : Return "╗"
        Case 188 : Return "╝"
        Case 189 : Return "╜"
        Case 190 : Return "╛"
        Case 191 : Return "┐"
        Case 192 : Return "└"
        Case 193 : Return "┴"
        Case 194 : Return "┬"
        Case 195 : Return "├"
        Case 196 : Return "─"
        Case 197 : Return "┼"
        Case 198 : Return "╞"
        Case 199 : Return "╟"
        Case 200 : Return "╚"
        Case 201 : Return "╔"
        Case 202 : Return "╩"
        Case 203 : Return "╦"
        Case 204 : Return "╠"
        Case 205 : Return "═"
        Case 206 : Return "╬"
        Case 207 : Return "╧"
        Case 208 : Return "╨"
        Case 209 : Return "╤"
        Case 210 : Return "╥"
        Case 211 : Return "╙"
        Case 212 : Return "╘"
        Case 213 : Return "╒"
        Case 214 : Return "╓"
        Case 215 : Return "╫"
        Case 216 : Return "╪"
        Case 217 : Return "┘"
        Case 218 : Return "┌"
        Case 219 : Return "█"
        Case 220 : Return "▄"
        Case 221 : Return "▌"
        Case 222 : Return "▐"
        Case 223 : Return "▀"
        Case 224 : Return "α"
        Case 225 : Return "ß"
        Case 226 : Return "Γ"
        Case 227 : Return "π"
        Case 228 : Return "Σ"
        Case 229 : Return "σ"
        Case 230 : Return "µ"
        Case 231 : Return "τ"
        Case 232 : Return "Φ"
        Case 233 : Return "Θ"
        Case 234 : Return "Ω"
        Case 235 : Return "δ"
        Case 236 : Return "∞"
        Case 237 : Return "φ"
        Case 238 : Return "ε"
        Case 239 : Return "∩"
        Case 240 : Return "≡"
        Case 241 : Return "±"
        Case 242 : Return "≥"
        Case 243 : Return "≤"
        Case 244 : Return "⌠"
        Case 245 : Return "⌡"
        Case 246 : Return "÷"
        Case 247 : Return "≈"
        Case 248 : Return "°"
        Case 249 : Return "●" ' ?
        Case 250 : Return "·"
        Case 251 : Return "√"
        Case 252 : Return "ⁿ"
        Case 253 : Return "²"
        Case 254 : Return "■"
        Case 255 : Return " "

        Case Else
          Return Chr(value)
      End Select

    End Function

#Region "STRING"

    Public Shared Function QBString(count As Double, char%) As String
      Return New String(QBChr(char%)(0), CInt(count))
    End Function

    Public Shared Function QBString(count%, char%) As String
      Return New String(QBChr(char%)(0), count%)
    End Function

    Public Shared Function QBString(count%, char$) As String
      Return New String(char$(0), count%)
    End Function

#End Region

    ' Arrays and Data

    Private Shared ReadOnly m_data As New List(Of String)
    Private Shared m_dataIndex As Integer = 0

    Public Shared Sub DATA(ParamArray values%())
      For Each value In values
        m_data.Add(value.ToString)
      Next
    End Sub

    Public Shared Sub DATA(ParamArray values$())
      For Each value In values$
        m_data.Add(value)
      Next
    End Sub

    Public Shared Sub READ(ByRef value$)
      If m_dataIndex < m_data.Count Then
        value$ = m_data(m_dataIndex)
        m_dataIndex += 1
      Else
        Throw New IndexOutOfRangeException()
      End If
    End Sub

    Public Shared Sub READ(ByRef value%)
      If m_dataIndex < m_data.Count Then
        value = CInt(m_data(m_dataIndex))
        m_dataIndex += 1
      Else
        Throw New IndexOutOfRangeException()
      End If
    End Sub

    Public Shared Sub READ(ByRef value&)
      If m_dataIndex < m_data.Count Then
        value = CLng(m_data(m_dataIndex))
        m_dataIndex += 1
      Else
        Throw New IndexOutOfRangeException()
      End If
    End Sub

    Public Shared Sub RESTORE()
      m_dataIndex = 0
    End Sub

    ' Math

    '#Region "ABS"

    '    Public Shared Function ABS(value As Integer) As Integer
    '      Return Math.Abs(value)
    '    End Function

    '    Public Shared Function ABS(value As String) As Integer
    '      Return Math.Abs(CInt(value))
    '    End Function

    '    Public Shared Function ABS(value As Boolean) As Integer
    '      Return Math.Abs(CInt(value))
    '    End Function

    '    Public Shared Function ABS(value As Long) As Long
    '      Return Math.Abs(value)
    '    End Function

    '#End Region

    ' Simple I/O

    'Public Shared Function INPUT$(n As Long, Optional filenumber% = -1)
    '  If filenumber% = -1 Then
    '    Dim resuLtKey$ = ""
    '    Do
    '      Dim A$ = INKEY()
    '      If A$ <> "" Then
    '        resuLtKey$ &= A$ : A$ = ""
    '        If Len(resuLtKey$) >= n Then
    '          Return resuLtKey$
    '        End If
    '      End If
    '    Loop
    '  Else
    '    Throw New NotImplementedException()
    '  End If
    'End Function


    'Public Shared Function TAB(column%) As String
    '  If column% <= 32767 Then
    '    If column% < 0 Then
    '      column% = 1
    '    Else
    '      column% = column% Mod 80
    '    End If
    '    Dim current% = System.Console.CursorLeft + 1
    '    Dim count% = column% - current%
    '    Dim resuLtKey$ = ""
    '    If count% < 0 Then
    '      resuLtKey$ = vbCrLf : count% = column%
    '    End If
    '    resuLtKey$ &= Space(count%)
    '    Return resuLtKey$
    '  Else
    '    Throw New ArgumentException
    '  End If
    'End Function

    ' Error Trapping

    ' Timing, Date, and Time

    Public Shared Function QBDate() As String
      Return Today.ToString("MM-dd-yyyy")
    End Function

    Public Shared Function QBTime() As String
      Return Now.ToString("HH:mm:ss")
    End Function

    'Public Shared Sub QBTime(value As String)
    '  'NOTE: Set the time???? (Is this even possible?)
    'End Sub

    Public Shared Function QBTimer() As Single
      Return CSng(DateDiff(DateInterval.Second, New Date(Now.Year, Now.Month, Now.Day), Now)) '+ ((Now.Millisecond \ 10) * 0.01))
    End Function

  End Class

End Namespace