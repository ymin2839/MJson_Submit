Public Class Form

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Dim Url = "https://api.coinone.co.kr/v2/transaction/krw/history/"
        MsgBox(Url)

        Dim Respone = CMJson.GetMJson2(Url)

        Console.WriteLine(Respone)

        MsgBox(Respone)

    End Sub
End Class