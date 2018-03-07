Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports Newtonsoft.Json

Public Class CMJson

    Private Shared m_strRespon As String = ""
    Public Shared Property Respon() As String
        Get
            Return m_strRespon
        End Get
        Set(value As String)
        End Set

    End Property

    Private Shared m_strReqUrl As String = ""
    Public Shared Property ReqUrl() As String
        Get
            Return m_strReqUrl
        End Get
        Set(value As String)
            m_strReqUrl = value
        End Set

    End Property

    Private Shared m_strAccessToken As String = "afac0a5f-556d-4f96-bc6d-0cf88b756b89"
    Private Shared m_strSecretKey As String = "1f5b0b21-1e31-42f7-ae54-70067af97d52"

    Public Shared Function GetMJson2(strUrl As String) As String

        Try
            ReqUrl = strUrl

            Dim encodePayload As String = Get_Encode_Payload()
            Dim encodeSignature As String = Get_Encode_Signature()
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(encodePayload)

            Dim s1 = encodePayload
            Dim s2 = encodeSignature

            Dim request = CType(WebRequest.Create(ReqUrl), HttpWebRequest)
            request.Method = "POST"
            request.ContentType = "application/json"
            request.Headers.Add("X-COINONE-PAYLOAD", encodePayload)
            request.Headers.Add("X-COINONE-SIGNATURE", encodeSignature)

            Dim s3 = request.Headers.ToString()

            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()

            Dim response As WebResponse = request.GetResponse()

            dataStream = response.GetResponseStream()

            Dim reader As New StreamReader(dataStream)

            m_strRespon = reader.ReadToEnd()

            reader.Close()
            dataStream.Close()
            response.Close()

            Return m_strRespon
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function Get_Encode_Signature() As String
        Dim strKey As String = m_strSecretKey
        Dim strMsg As String = Get_Encode_Payload()

        Dim byteKey As Byte() = Encoding.UTF8.GetBytes(strKey.ToUpper())
        Dim byteMsg As Byte() = Encoding.UTF8.GetBytes(strMsg)

        Dim signature = Encode_hmacsha512(byteKey, byteMsg)

        Return signature
    End Function

    Public Shared Function Get_Encode_Payload() As String
        Dim striData As String = "{""access_token"": """ & m_strAccessToken & """, ""nonce"": 1520222501670}"

        Dim byteDump As Byte() = Encoding.UTF8.GetBytes(striData)

        Dim payload_enc As String = Convert.ToBase64String(byteDump)

        Return payload_enc
    End Function

    Public Shared Function Encode_hmacsha512(ByVal byteKey As Byte(), ByVal byteMsg As Byte()) As String

        Dim hmacsha512 As New HMACSHA512(byteKey)

        Dim hashValue As Byte() = hmacsha512.ComputeHash(byteMsg)

        Dim stringBuilder As New StringBuilder()
        For i As Integer = 0 To hashValue.Length - 1
            stringBuilder.Append(hashValue(i).ToString("x2"))
        Next

        Dim encode = stringBuilder.ToString()

        hmacsha512.Clear()

        Return encode
    End Function
End Class

