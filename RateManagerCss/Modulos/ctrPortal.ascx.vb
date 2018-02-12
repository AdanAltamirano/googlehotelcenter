Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos
Imports Portal.Catalogos.Facade
Imports Portal.Hotel
Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient

Partial Class ctrPortal
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Const KEY_HOTELID = "HotelId"
    Const KEY_opcion = "opcion"
    Const KEY_IdRatePlan = "IdRatePlan"

    Public Property m_HotelId() As Integer
        Get
            Return ViewState(KEY_HOTELID)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_HOTELID) = Value
        End Set
    End Property

    Public Property m_opcion() As Integer
        Get
            Return ViewState(KEY_opcion)
        End Get
        Set(ByVal Value As Integer)
            ViewState(KEY_opcion) = Value
        End Set
    End Property

    Public Property m_IdRatePlan() As String
        Get
            Return ViewState(KEY_IdRatePlan)
        End Get
        Set(ByVal Value As String)
            ViewState(KEY_IdRatePlan) = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack Then
            CType(Me.Page, PaginaBase).RegisterStartupScript("HidePortals", "<script>ShowHidePortals(0);</script>")
            Me.FillPortals()
            Me.Limpiar()
        End If

        chkPortalTodo.Attributes.Add("onclick", "javascript:checkAll('" & Me.chkPortalTodo.ClientID & "','" & cblPortal.ClientID & "');")
        cblPortal.Attributes.Add("onclick", "javascript:UncheckAll('" & Me.chkPortalTodo.ClientID & "','" & cblPortal.ClientID & "');")

    End Sub

    Private Sub FillPortals()

        Dim data As clsCommonPortales = Nothing

        Me.cblPortal.Items.Clear()

        With New clsFacadePortales()
            Dim currentPage As PaginaBase = CType(Me.Page, PaginaBase)

            Dim currentCorporate As Integer = 0
            Dim currentAsociation As Integer = 0


            If AppSettings("IdAsociation") IsNot Nothing AndAlso Integer.TryParse(AppSettings("IdAsociation"), currentAsociation) Then

                data = LocalGetPortalsByAsociation()
            Else
                If (currentPage.IsSupervisor) Then
                    data = .GetPortales(currentPage.cInfoActual.Empresa)
                ElseIf (currentPage.isUserChain AndAlso currentPage.IdCorporativoUserChain > 0) Then
                    data = .GetPortalsByCorporate(currentPage.IdCorporativoUserChain)
                ElseIf (currentPage.IsUsuarioHotelAssociation AndAlso currentPage.IdAsociation > 0) Then
                    data = .GetPortalsByAsociation(currentPage.IdAsociation)
                Else
                    data = .GetPortals(onlyPublics:=True, corporate:=currentPage.cInfoActual.IdCorporate, asociation:=currentPage.cInfoActual.IdAsociation)
                End If
            End If



        End With

        If data IsNot Nothing AndAlso data.Tables.Contains(data.TABLA_PORTALES) Then
            For Each row As DataRow In data.Tables(data.TABLA_PORTALES).Rows
                Dim item As New ListItem()
                item.Text = StrConv(row(data.FLD_NOMBRE), VbStrConv.ProperCase)
                item.Value = row(data.FLD_IDPORTAL).ToString()
                item.Selected = True
                cblPortal.Items.Add(item)
            Next
        End If

    End Sub

    Public Sub Limpiar()

        Me.chkPortalTodo.Checked = True

        For Each item As ListItem In cblPortal.Items
            item.Selected = True
        Next

    End Sub

    Public Sub LoadPortales(ByVal idHotel As Integer, ByVal opcion As Integer, ByVal IdRatePlan As String)

        Dim data As New AplicacionPortalData()

        Me.chkPortalTodo.Checked = False
        Me.cblPortal.ClearSelection()

        Me.m_HotelId = idHotel
        Me.m_opcion = opcion
        Me.m_IdRatePlan = IdRatePlan

        With New AplicacionPortalFacade
            data = .GetAplicacionPortalByIdHotel(m_HotelId, m_opcion, m_IdRatePlan)
        End With

        If data IsNot Nothing AndAlso data.Tables.Contains(data.TABLE_aplicacionPortal) Then

            Dim selectectionCount As Integer = 0
            For Each row As DataRow In data.Tables(data.TABLE_aplicacionPortal).Rows

                If row(data.FIELD_aplicar) = 1 Then
                    If row(data.FIELD_idPortal) = 0 Then
                        selectectionCount = cblPortal.Items.Count
                        For Each item As ListItem In cblPortal.Items
                            item.Selected = True
                        Next
                        Exit For
                    ElseIf cblPortal.Items.FindByValue(row(data.FIELD_idPortal).ToString()) IsNot Nothing Then
                        cblPortal.Items.FindByValue(row(data.FIELD_idPortal).ToString()).Selected = True
                        selectectionCount += 1
                    End If
                End If
            Next
            Me.chkPortalTodo.Checked = (selectectionCount = cblPortal.Items.Count)

        End If

        'Dim ds As New AplicacionPortalData
        'Dim dt As New DataTable, dr2 As DataRow
        'Dim i As Integer

        'm_HotelId = idHotel
        'm_opcion = opcion
        'm_IdRatePlan = IdRatePlan

        'Limpiar()

        'Dim columna As DataColumn = New DataColumn
        'columna.DataType = System.Type.GetType("System.Int32")
        'columna.ColumnName = "idPortal"

        'dt.Columns.Add(columna)
        'dt.Columns.Add("idHotel")
        'dt.Columns.Add("opcion")
        'dt.Columns.Add("IdRatePlan")
        'dt.Columns.Add("aplicar")

        ''Dim keys(0) As DataColumn
        ''keys(0) = columna
        ''dt.PrimaryKey = keys

        'With New AplicacionPortalFacade
        '    ds = .GetAplicacionPortalByIdHotel(m_HotelId, m_opcion, m_IdRatePlan)
        'End With

        'If Not ds Is Nothing Then
        '    For Each dr As DataRow In ds.Tables(AplicacionPortalData.TABLE_aplicacionPortal).Rows
        '        Try
        '            dr2 = dt.NewRow()
        '            dr2("idPortal") = dr("idPortal")
        '            dr2("idHotel") = dr("idHotel")
        '            dr2("opcion") = dr("opcion")
        '            dr2("IdRatePlan") = dr("IdRatePlan")
        '            dr2("aplicar") = dr("aplicar")
        '            dt.Rows.Add(dr2)
        '            'Catch ex As Exception
        '        Catch e As DataException

        '        End Try
        '    Next

        '    dr2 = Nothing
        '    dr2 = dt.Rows.Find(0)
        '    If Not dr2 Is Nothing Then
        '        If dr2("aplicar") = 1 Then
        '            Me.chkPortalTodo.Checked = True
        '            For i = 0 To cblPortal.Items.Count - 1
        '                cblPortal.Items(i).Selected = True

        '            Next
        '        End If

        '    Else
        '        For i = 0 To cblPortal.Items.Count - 1
        '            dr2 = Nothing
        '            dr2 = dt.Rows.Find(cblPortal.Items(i).Value)
        '            cblPortal.Items(i).Selected = False
        '            If Not dr2 Is Nothing Then
        '                If dr2("aplicar") = 1 Then
        '                    cblPortal.Items(i).Selected = True
        '                End If
        '            End If
        '        Next
        '    End If

        '    For i = 0 To cblPortal.Items.Count - 1
        '        If Not cblPortal.Items(i).Selected Then
        '            Me.chkPortalTodo.Checked = False
        '            Exit For
        '        End If
        '    Next
        'End If

    End Sub

    Public Function ModificarPortales(ByVal valorPortal As Boolean) As Integer
        Return InsertarPortales(True, m_HotelId, m_opcion, m_IdRatePlan, valorPortal)
        'Dim rPortal As DataRow
        'Dim i As Integer
        'Dim ds As New AplicacionPortalData
        'Dim dt As New DataTable, dr2 As DataRow
        'Dim dv As DataView

        'Dim columna As DataColumn = New DataColumn
        'columna.DataType = System.Type.GetType("System.Int32")
        'columna.ColumnName = "idPortal"

        'dt.Columns.Add(columna)
        'dt.Columns.Add("idHotel")
        'dt.Columns.Add("IdRatePlan")
        'dt.Columns.Add("aplicar")
        'dt.Columns.Add("idAplicacionPortal")

        'Dim keys(0) As DataColumn
        'keys(0) = columna
        'dt.PrimaryKey = keys

        'With New AplicacionPortalFacade
        '    ds = .GetAplicacionPortalByIdHotel(m_HotelId, m_opcion, m_IdRatePlan)
        'End With

        'If Not ds Is Nothing Then
        '    dv = ds.Tables(AplicacionPortalData.TABLE_aplicacionPortal).DefaultView()

        '    For Each dr As DataRow In ds.Tables(AplicacionPortalData.TABLE_aplicacionPortal).Rows
        '        dr2 = dt.NewRow()
        '        dr2("idAplicacionPortal") = dr("idAplicacionPortal")
        '        dr2("idPortal") = dr("idPortal")
        '        dr2("idHotel") = dr("idHotel")
        '        dr2("IdRatePlan") = dr("IdRatePlan")
        '        dr2("aplicar") = dr("aplicar")
        '        dt.Rows.Add(dr2)
        '    Next
        'End If

        'For i = 0 To cblPortal.Items.Count - 1
        '    dr2 = Nothing
        '    dr2 = dt.Rows.Find(cblPortal.Items(i).Value)

        '    If Not dr2 Is Nothing Then  'registro a modificar
        '        Dim reng As DataRow() = ds.Tables(ds.TABLE_aplicacionPortal).Select("idAplicacionPortal =" & dr2("idAplicacionPortal"))

        '        If reng.Length > 0 Then
        '            With reng(0)
        '                If valorPortal = True Then
        '                    .Item(ds.FIELD_aplicar) = cblPortal.Items(i).Selected
        '                Else
        '                    .Item(ds.FIELD_aplicar) = False
        '                End If

        '            End With
        '        End If
        '    Else 'registro nuevo
        '        If valorPortal = True Then
        '            If cblPortal.Items(i).Selected = True Then
        '                rPortal = ds.Tables(ds.TABLE_aplicacionPortal).NewRow()

        '                With rPortal
        '                    .Item(ds.FIELD_idHotel) = m_HotelId
        '                    .Item(ds.FIELD_idPortal) = cblPortal.Items(i).Value
        '                    .Item(ds.FIELD_opcion) = m_opcion
        '                    .Item(ds.FIELD_IdRatePlan) = m_IdRatePlan.Trim
        '                    .Item(ds.FIELD_aplicar) = cblPortal.Items(i).Selected
        '                End With
        '                ds.Tables(ds.TABLE_aplicacionPortal).Rows.Add(rPortal)
        '            End If
        '        End If
        '    End If
        'Next

        'With New AplicacionPortalFacade
        '    If .ModificarAplicacionPortal(ds, m_opcion) Then
        '        Limpiar()
        '        Return 0
        '    End If
        'End With

        'Return 2
    End Function
    Public Function InsertarPortales(ByVal Modify As Boolean, ByVal idHotel As Integer, ByVal opcion As Integer, ByVal IdRatePlan As String, ByVal valorPortal As Boolean) As Integer

        Dim rPortal As DataRow
        Dim data As New AplicacionPortalData()

        If valorPortal = True Then
            With New AplicacionPortalFacade
                data = .GetAplicacionPortalByIdHotel(idHotel, opcion, IdRatePlan.Trim.ToUpper)
            End With
            data.AcceptChanges()

            Dim rows As DataRow()
            Dim newRow As DataRow
            With data.Tables(data.TABLE_aplicacionPortal)
                For Each item As ListItem In cblPortal.Items
                    rows = .Select(data.FIELD_idPortal + "='" + item.Value + "'")
                    If rows.Length = 0 AndAlso (Me.chkPortalTodo.Checked OrElse item.Selected) Then
                        newRow = .NewRow()
                        newRow(data.FIELD_idHotel) = idHotel
                        newRow(data.FIELD_idPortal) = item.Value
                        newRow(data.FIELD_opcion) = opcion
                        newRow(data.FIELD_IdRatePlan) = IdRatePlan.Trim.ToUpper
                        newRow(data.FIELD_aplicar) = True
                        .Rows.Add(newRow)
                    ElseIf (rows.Length > 0 AndAlso Not Me.chkPortalTodo.Checked AndAlso Not item.Selected) Then
                        rows(0).Delete()
                    End If
                Next
                rows = .Select(data.FIELD_idPortal + "='0'")
                If rows.Length > 0 Then rows(0).Delete()
            End With

            With New AplicacionPortalFacade
                If .InsertarAplicacionPortal(data) Then
                    Limpiar()
                    Return 0
                End If
            End With
        Else
            Return 0
        End If

        Return 2
    End Function

    Public Function InsertarPortales(ByVal idHotel As Integer, ByVal opcion As Integer, ByVal IdRatePlan As String, ByVal valorPortal As Boolean) As Integer
        InsertarPortales(False, idHotel, opcion, IdRatePlan, valorPortal)
    End Function

    Public Function EliminarPortales(ByVal IdHotel As Integer, ByVal opcion As Integer, ByVal IdRatePlan As String) As Integer
        With New AplicacionPortalFacade
            If .EliminarAplicacionPortal(IdHotel, opcion, IdRatePlan) Then
                Limpiar()
                Return 0
            End If
        End With

        Return 2
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblPortal.Text = PortalCulture.GetString("01009")
        chkPortalTodo.Text = PortalCulture.GetString("01011")
        If chkPortalTodo.Checked = True Then
            CType(Me.Page, PaginaBase).RegisterStartupScript("HidePortals", "<script>ShowHidePortals(0);</script>")
        End If
    End Sub

    Public Function validaSeleccionPortal(ByVal valor As Boolean) As Boolean
        Dim i As Integer

        If valor = True Then
            For i = 0 To cblPortal.Items.Count - 1
                If cblPortal.Items(i).Selected = True Then
                    Return True
                End If
            Next
        End If

        Return False
    End Function


    Private Function LocalGetPortalsByAsociation() As clsCommonPortales
        Dim data As New clsCommonPortales

        Try
            Dim idAsociacion As Integer = 0

            Dim cmdText As String = "spGetPotalsAssociation"
            Dim cnn As New SqlConnection(AppSettings("PortalConnection"))
            Dim cmd As New SqlCommand(cmdText, cnn)
            Dim ada As New SqlDataAdapter
            cmd.CommandType = CommandType.StoredProcedure
            If AppSettings("PortalConnection") <> String.Empty AndAlso AppSettings("IdAsociation") IsNot Nothing AndAlso Integer.TryParse(AppSettings("IdAsociation"), idAsociacion) Then
                Try

                    cmd.Parameters.Add(New SqlParameter("@idAsociacion", idAsociacion))
                    ada.SelectCommand = cmd
                    ada.TableMappings.Add("Table", "Portales")
                    ada.Fill(data)
                Catch ex As Exception
                Finally
                    If cnn IsNot Nothing AndAlso cnn.State <> ConnectionState.Closed Then
                        cnn.Close()
                    End If
                End Try
            End If
        Catch ex As Exception
        End Try
        Return data
    End Function

End Class
