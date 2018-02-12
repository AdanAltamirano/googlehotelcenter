Imports System.IO

Imports Portal.Common
Imports Portal.Facade

Partial Public Class CtrRatePlanContain
    Inherits UserControlBase

    Private sr As StringWriter
    Shared CtrlIdioma1 As CtrlIdiomaPortalFCK

    Public Property ds() As DataSet
        Get
            Return ViewState("ds")
        End Get
        Set(ByVal Value As DataSet)
            ViewState("ds") = Value
        End Set
    End Property

    Public Property IdIndice() As Integer
        Get
            Return ViewState("IdIndice")
        End Get
        Set(ByVal value As Integer)
            ViewState("IdIndice") = value
        End Set
    End Property



    Public Property IdHotel() As Integer
        Get
            Return ViewState("IdHotel")
        End Get
        Set(ByVal value As Integer)
            ViewState("IdHotel") = value
        End Set
    End Property

    Public Property IdCode() As String
        Get
            Return ViewState("IdCode")
        End Get
        Set(ByVal value As String)
            ViewState("IdCode") = value
        End Set
    End Property


    Public Property IdRatePlan() As String
        Get
            Return ViewState("IdRatePlan")
        End Get
        Set(ByVal value As String)
            ViewState("IdRatePlan") = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

        End If
    End Sub

    Function getDataXML() As String
        Dim ds As New DataSet
        With New SystemDictionaryPortal()
            ds = .GetDictionaryAllByID(IdHotel, IdIndice)
        End With
        Return ds.GetXml
    End Function


    Function LoadDictionaryAllPortals(ByRef sdata As String) As Boolean
        Dim dv As DataView

        With New SystemDictionaryPortal()
            ds = .GetDictionaryAllByID(IdHotel, IdIndice)
        End With

        dv = ds.Tables(1).DefaultView
        'dv.RowFilter = String.Format("idIdioma = {0}", PortalCulture.GetIDCulture)
        DG_Datos.DataSource = dv
        DG_Datos.DataBind()
        Return If(dv.Count = 0, False, True)
    End Function

    Function RegexHtmlToText(ByVal src As String) As String
        Dim sHtml As String
        Const LF As String = Chr(10)
        Const CR As String = Chr(13)
        Dim pattern As String = "\s+"
        Dim replacement As String = " "
        Dim rgx As New Regex(pattern)

        Try
            sHtml = src
            sHtml = sHtml.Replace("&amp;", "&")
            sHtml = sHtml.Replace("&aacute;", "á").Replace("&eacute;", "é").Replace("&iacute;", "í")
            sHtml = sHtml.Replace("&oacute;", "ó").Replace("&uacute;", "ú").Replace("&Aacute;", "Á")
            sHtml = sHtml.Replace("&Eacute;", "É").Replace("&Iacute;", "Í").Replace("&Oacute;", "Ó")
            sHtml = sHtml.Replace("&Uacute;", "Ú").Replace("&ntilde;", "ñ").Replace("&Ntilde;", "Ñ")
            sHtml = sHtml.Replace("&iquest;", "¿").Replace("¡", "&iexcl;").Replace("'", "\'").Replace("""", "\""")
            sHtml = Regex.Replace(sHtml, "&(?ni:\#((x([\dA-F]){1,5})|(104857[0-5]|10485[0-6]\d|1048[0-4]\d\d|104[0-7]\d{3}|10[0-3]\d{4}|0?\d{1,6}))|([A-Za-z\d.]{2,31}));|<[^>]*>", "")
            sHtml = sHtml.Replace(vbCr, "").Replace(vbCrLf, "").Replace(vbLf, "")
            sHtml = rgx.Replace(sHtml, replacement)

            src = sHtml
        Catch ex As Exception
        End Try
        Return (src)
    End Function

    Public Function GetShortContainer(ByVal src As String, ByVal size As Integer) As String
        Dim cont As Integer
        Dim scopy As String = ""
        Dim isLonger As Boolean

        If size = 0 Then Return src
        src = RegexHtmlToText(src)
        If String.IsNullOrEmpty(src.Trim) Then src = PortalCulture.GetString("01245")

        Try
            src = src.Trim
            isLonger = False
            For i As Integer = 0 To src.Length - 1
                If src.Substring(i, 1) = " " Then
                    cont += 1
                    If cont = size Then
                        isLonger = True
                        Exit For
                    End If
                End If
                scopy &= src.Substring(i, 1)
            Next
            scopy = scopy.Trim
            scopy &= IIf(isLonger, " ...", "")
        Catch ex As Exception
            scopy = src
        End Try

        Return scopy
    End Function

    Public Function Clear() As Boolean
        DG_Datos.DataSource = Nothing
        DG_Datos.DataBind()
    End Function

    Public Function LoadContainPortal(ByVal idhotels As Integer, ByVal idDicionario As Integer, ByVal rateplan As String, ByVal code As String, ByRef isValid As Boolean) As Boolean
        ds = New DataSet
        Dim dv As DataView
        Dim hr As Boolean
        Dim sdata As String = ""

        hr = True
        IdHotel = idhotels
        IdIndice = idDicionario
        Idcode = code
        IdRatePlan = rateplan
        DG_Datos.EditItemIndex = -1
        hr = LoadDictionaryAllPortals(sdata)
        Return hr
    End Function

    Public Function UpdateDicctionaryPortals1(ByVal idDicc As Integer) As Boolean
        Dim dv As DataView
        Dim textoEng As String
        Dim textoEsp As String

        Dim idPortal As Integer
        For Each dr As DataRow In ds.Tables(2).Rows
            idPortal = dr("idportal")
            dv = ds.Tables(1).DefaultView
            dv.RowFilter = String.Format("idportal= {0}", idPortal)
            textoEsp = ""
            textoEng = ""
            For Each drv As DataRowView In dv
                If drv("idIdioma") = 1 Then
                    textoEsp = drv("texto")
                End If
                If drv("idIdioma") = 2 Then
                    textoEng = drv("texto")
                End If
            Next

            If Not CtrlIdioma1 Is Nothing Then
                If String.IsNullOrEmpty(RegexHtmlToText(textoEsp).Trim) And String.IsNullOrEmpty(RegexHtmlToText(textoEng).Trim) Then
                    CtrlIdioma1.Delete(idPortal, idDicc)
                Else
                    CtrlIdioma1.IdPortal = idPortal
                    CtrlIdioma1.Update(textoEng, textoEsp, "", "", idDicc)
                End If
            End If

        Next

    End Function

    Private Sub DG_Datos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DG_Datos.ItemDataBound
        Dim btnUp As Button
        Dim ImageButton1 As ImageButton

        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.EditItem Then
            e.Item.Cells(0).CssClass = "clslabel"
            ImageButton1 = e.Item.Cells(2).FindControl("ImageButton3")
            If Not ImageButton1 Is Nothing Then
                ImageButton1.ToolTip = PortalCulture.GetString("A00666")
            End If

            'ImageButton1.ToolTip = PortalCulture.GetString("A00666")
        End If

        '// Edicion del grid.
        If e.Item.ItemType = ListItemType.EditItem Then

            Dim ImgBtnDelete As LinkButton
            Dim panel As HtmlTableRow
            Dim lblEdit As Label

            e.Item.Cells(2).FindControl("LlbEdit")
            CtrlIdioma1 = e.Item.Cells(2).FindControl("CtrlIdiomaPortalFCK1")
            panel = e.Item.Cells(2).FindControl("pnlUnpublishedContain")

            lblEdit = e.Item.Cells(2).FindControl("LlbEdit")
            lblEdit.Text = PortalCulture.GetString("01250")

            btnUp = e.Item.Cells(2).FindControl("Button1")
            If Not btnUp Is Nothing Then
                btnUp.Text = PortalCulture.GetString("00008")
            End If
            btnUp = e.Item.Cells(2).FindControl("BtnUpdate2")
            If Not btnUp Is Nothing Then
                btnUp.Text = PortalCulture.GetString("00095")
            End If
            btnUp = e.Item.Cells(2).FindControl("BtnPublish")
            If Not btnUp Is Nothing Then
                btnUp.Text = PortalCulture.GetString("01364")
            End If

            If e.Item.ItemIndex <= ds.Tables(DictionaryDataPortal.TablaDiccionario).Rows.Count - 1 Then
                Dim dv As DataView
                Dim dr As DataRow
                dv = ds.Tables(DictionaryDataPortal.TablaDiccionario).DefaultView
                dr = dv(e.Item.ItemIndex).Row
                If Not CtrlIdioma1 Is Nothing Then
                    CtrlIdioma1.IdIdioma = dr.Item("IdIdiomaPortal")
                    CtrlIdioma1.IdPortal = dr.Item(DictionaryDataPortal.FIELD_IdPortal)
                    CtrlIdioma1.SoloIdiomaDefault()
                    For Each dr In ds.Tables(1).Select(String.Format("idIdioma= 1 And idportal= {0}", CtrlIdioma1.IdPortal))
                        CtrlIdioma1.textoEspañol = dr(DictionaryDataPortal.FIELD_Texto)
                    Next
                    For Each dr In ds.Tables(1).Select(String.Format("idIdioma= 2 And idportal= {0}", CtrlIdioma1.IdPortal))
                        CtrlIdioma1.textoIngles = dr(DictionaryDataPortal.FIELD_Texto)
                    Next

                    If panel IsNot Nothing Then panel.Visible = (Not Convert.ToBoolean(dr(DictionaryDataPortal.Published_FIELD)))

                End If
            End If
        End If
    End Sub

    Private Sub DG_Datos_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DG_Datos.EditCommand
        Dim sdata As String = ""
        Me.DG_Datos.EditItemIndex = e.Item.ItemIndex
        LoadDictionaryAllPortals(sdata)
    End Sub

    Private Sub DG_Datos_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DG_Datos.ItemCommand
        '// cancela el row
        Dim sData As String = ""
        If e.CommandName = "UpThis" Or e.CommandName = "DownThis" Then            
        ElseIf e.CommandName = "Cancel" Then
            Me.DG_Datos.EditItemIndex = -1
            LoadDictionaryAllPortals(sData)
        ElseIf e.CommandName.ToLower = "publish" Then
            DG_Datos_UpdateCommand(source, e)
        End If
    End Sub

    Private Sub DG_Datos_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DG_Datos.UpdateCommand

        Dim publish As Boolean = (e.CommandName.ToLower() = "publish")

        Dim alfo As Integer = 0
        Dim idPortal As Integer
        Dim sData As String = ""
        Dim sDataPrev As String = ""

        sData = getDataXML()
        CtrlIdioma1 = e.Item.Cells(2).FindControl("CtrlIdiomaPortalFCK1")
        idPortal = e.CommandArgument
        If Not CtrlIdioma1 Is Nothing Then
            CtrlIdioma1.IdPortal = idPortal
            CtrlIdioma1.Update(IdIndice, publish)
        End If
        Me.DG_Datos.EditItemIndex = -1
        LoadDictionaryAllPortals(sData)

        CType(Me.Page, PaginaBase).guardalog("/Pages/RatePlanContentPortal.aspx", If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar), "Se modificó contenido para plan tarifario " & IdCode, "", sDataPrev, sData)
        If Me.CtrlIdioma1.HasChanges Then CType(Me.Page, PaginaBase).NotifyContentModification("Contenido de portal para plan tarifario con el codigo " & IdCode)
    End Sub

End Class