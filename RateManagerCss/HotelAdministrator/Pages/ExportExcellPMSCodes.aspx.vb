Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Xml

Public Class ExportExcellPMSCodes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                ' Formatear la fecha a fecha corta y hora

                Dim codePMS As String = Session("PMSCodeId").ToString()

                Dim hPMSInfo As DataSet = getHotelPMSInfo(codePMS)

                Dim hotelName As String = Session("PMSHotelName").ToString()
                Dim generatedAt As String = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss")

                Response.Clear()
                Response.Buffer = True
                Response.ClearHeaders()
                Response.CacheControl = "no-cache"
                Response.AddHeader("Pragma", "no-cache")
                Response.AddHeader("content-disposition", "attachment;filename=" + "PMSCodes_" + hotelName + "_" + generatedAt + ".xls")
                Response.Charset = ""
                Response.ContentEncoding = System.Text.Encoding.Default

                txtHotelName.Text = hotelName
                txtCode.Text = hPMSInfo.Tables(0).Rows(0)("idEnterprise")
                txtUser.Text = hPMSInfo.Tables(0).Rows(0)("userName")
                txtPass.Text = hPMSInfo.Tables(0).Rows(0)("password")
                txtEndPoint.Text = If(String.IsNullOrWhiteSpace(hPMSInfo.Tables(0).Rows(0)("URL")), AppSettings("Default_PMS_URL"), hPMSInfo.Tables(0).Rows(0)("URL"))

                Response.ContentType = "application/ms-excel"
                Dim stringWrite As New System.IO.StringWriter
                Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)

                Dim dtRooms As New DataTable
                dtRooms.Merge(CType(Session("PMSRooms"), DataTable))
                Dim dtRatePlans As New DataTable
                dtRatePlans.Merge(CType(Session("PMSRatePlans"), DataTable))

                generalTable.RenderControl(htmlWrite)
                tblRooms.RenderControl(htmlWrite)
                With dgRooms
                    .DataSource = dtRooms
                    .DataBind()
                    .RenderControl(htmlWrite)
                End With

                tblPlans.RenderControl(htmlWrite)
                With dgRatePlans
                    .DataSource = dtRatePlans
                    .DataBind()
                    .RenderControl(htmlWrite)
                End With

                Response.Write(stringWrite.ToString)
            End If
        Catch ex As Exception
            Response.Clear()
        Finally
            Response.End()
        End Try

    End Sub

    Private Function getHotelPMSInfo(ByVal idEmpresa As String) As DataSet
        ' [spGetUsuarioConectividad] 0, '15210', 300, '', ''
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spGetUsuarioConectividad", conection)

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@refpoint", idEmpresa)) ' Se usa como comodin en el sp
            .Parameters.Add(New SqlParameter("@movimiento", 300)) ' Movimiento 300 : Busqueda por idEmpresa o IdHotel
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)

        If dRes IsNot Nothing And dRes.Tables(0).Rows.Count > 0 Then
            Dim filteredEnterprise As DataTable = dRes.Tables(0).Select("ISNULL(TypePms, 0) = 0").CopyToDataTable
            If IsDBNull(filteredEnterprise.Rows(0).Item("URL")) Then
                filteredEnterprise.Rows(0).Item("URL") = ""
            End If

            dRes = New DataSet
            dRes.Tables.Add(filteredEnterprise)
            dRes.AcceptChanges()

        End If

        Return dRes
    End Function


End Class