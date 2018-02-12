Imports System.IO

Partial Class exporter
    Inherits System.Web.UI.Page

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Dim dt As DataTable
    Dim dr As DataRow

    Private Sub InicializarTabla()
        dt = New DataTable("wzSeamlessLog")
        With dt.Columns
            .Add(DataAccess.Seamless.Campos.PN, GetType(String))
            .Add(DataAccess.Seamless.Campos.DS, GetType(String))
            .Add(DataAccess.Seamless.Campos.TS, GetType(String))
            .Add(DataAccess.Seamless.Campos.IN, GetType(String))
            .Add(DataAccess.Seamless.Campos.OT, GetType(String))
            .Add(DataAccess.Seamless.Campos.NA, GetType(Integer))
            .Add(DataAccess.Seamless.Campos.NC, GetType(Integer))
            .Add(DataAccess.Seamless.Campos.RSP, GetType(String))
            .Add(DataAccess.Seamless.Campos.TX, GetType(String))
            .Add(DataAccess.Seamless.Campos.MSG, GetType(String))
            .Add(DataAccess.Seamless.Campos.GDS, GetType(String))
            .Add(DataAccess.Seamless.Campos.AGY, GetType(String))
        End With
    End Sub

    Private Sub ParsearLog(ByVal sLog As String)
        Dim s As String() = sLog.Split(vbLf)
        Dim sPet, sTx, sST, sRes, sMsg As String
        Dim bMA As Boolean
        For j As Long = 0 To s.Length - 2
            sPet = s(j).Substring(25, 5)
            If sPet = "+ /TX" Then
                sTx = GetCampo("/TX -", s(j))
                bMA = IIf(sTx = "MA", True, False)
                Dim k As Long = (j + 1)
                Dim sTmp As String
                Do
                    sTmp = s(k).Substring(25, 5)
                    If sTmp = "@ /TX" Then
                        If bMA Then
                            sRes = GetRespuesta(s(k))
                        Else
                            sRes = GetCampo("/ST -", s(k))
                        End If
                        sST = GetCampo("/ST -", s(k))
                        If sST = "E" Then
                            sMsg = GetCampo("/O2 -", s(k))
                        Else
                            sMsg = ""
                        End If
                    End If
                    k += 1
                Loop Until sTmp = "@ /TX"
                AgregarPeticion(s(j), sRes, sST, sMsg, bMA)
            End If
        Next
    End Sub

    Private Function GetCampo(ByVal sCampo As String, ByVal sCad As String) As String
        Dim s As String
        If sCad.IndexOf(sCampo) >= 0 Then
            s = sCad.Substring(sCad.IndexOf(sCampo))
            GetCampo = s.Substring(5, s.IndexOf(vbCr) - 5)
        Else
            GetCampo = ""
        End If
    End Function

    Private Function GetRespuesta(ByVal sCad As String) As String
        Dim s As String = ""
        Dim str As String()
        If sCad.IndexOf("/PRA-") >= 0 Then
            s = sCad.Substring(sCad.IndexOf("/PRA-"))
            s = s.Substring(5, s.IndexOf(vbCr) - 5)
            str = s.Split(vbFormFeed)
            If str.Length > 3 Then
                GetRespuesta = IIf(str(2) Is Nothing, "", str(2))
            Else
                GetRespuesta = ""
            End If
        End If
    End Function

    Private Sub AgregarPeticion(ByVal sCad As String, ByVal sResp As String, ByVal sST As String, ByVal sMsg As String, Optional ByVal bMA As Boolean = False)
        Dim iCont As Integer = 1
        If bMA Then iCont = 8
        Dim sPI As String = ""
        If sResp Is Nothing Then sResp = ""
        Dim sNA, sNC As String
        Dim iNA As Integer = 0
        Dim iNC As Integer = 0
        For j As Integer = 1 To iCont
            If j = 1 Then
                sPI = GetCampo("/PI -", sCad)
            Else
                sPI = GetCampo("/PI" & j.ToString & "-", sCad)
            End If
            If sPI = "" Then Exit For
            sNA = GetCampo("/NA -", sCad)
            sNC = GetCampo("/NC -", sCad)
            If sNA <> "" Then iNA = CInt(sNA)
            If sNC <> "" Then iNC = CInt(sNC)
            dr = dt.NewRow
            With dr
                .Item(DataAccess.Seamless.Campos.PN) = sPI
                .Item(DataAccess.Seamless.Campos.DS) = GetCampo("/DS -", sCad)
                .Item(DataAccess.Seamless.Campos.TS) = GetCampo("/TS -", sCad)
                .Item(DataAccess.Seamless.Campos.IN) = GetCampo("/IN -", sCad)
                .Item(DataAccess.Seamless.Campos.OT) = GetCampo("/OT -", sCad)
                .Item(DataAccess.Seamless.Campos.NA) = iNA
                .Item(DataAccess.Seamless.Campos.NC) = iNC
                .Item(DataAccess.Seamless.Campos.RSP) = sResp
                .Item(DataAccess.Seamless.Campos.TX) = GetCampo("/TX -", sCad)
                .Item(DataAccess.Seamless.Campos.MSG) = sMsg
                .Item(DataAccess.Seamless.Campos.GDS) = GetCampo("/SY -", sCad)
                .Item(DataAccess.Seamless.Campos.AGY) = GetCampo("/BS -", sCad)
            End With
            dt.Rows.Add(dr)
        Next
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ini, logs, indice As String

        ini = Request.QueryString("ini")
        logs = Request.QueryString("logs")
        indice = Request.QueryString("ind")
        Dim dIni, dFin As DateTime
        dIni = CDate(ini).AddDays(CInt(indice) - 1)
        'Inicializo la tabla para insertar los logs ...
        InicializarTabla()
        Dim sFile As String = ""
        sFile = Config.RutaLogs & Config.PrefijoLogs & dIni.ToString("dd") & dIni.ToString("MM") & ".log"
        Insertar(sFile)
        Response.Write(indice)
    End Sub

    Private Sub Insertar(ByVal sArchivo As String)
        If File.Exists(sArchivo) Then
            Dim sArch As String = sArchivo.Replace(".log", "").Replace(Config.RutaLogs, "")
            Dim iAnio As Integer = File.GetCreationTime(sArchivo).Year
            Dim sLog As String
            sLog = File.OpenText(sArchivo).ReadToEnd
            ParsearLog(sLog)
            Dim cExLogs As New DataAccess.ExportedLogs
            If cExLogs.Validate(sArch, iAnio) Then
                If (New DataAccess.Seamless).Insert(dt) Then
                    cExLogs.Insert(sArch, iAnio)
                End If
            End If
        End If
    End Sub

End Class
