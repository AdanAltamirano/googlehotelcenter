Imports Portal.Catalogos.Common.Data
Imports Portal.Catalogos.Facade
Imports System.Text
Partial Class ctlConstructorGramatical
    Inherits System.Web.UI.UserControl

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

    Dim controles As Integer
    Dim arreglo() As String
    Dim arreglocad() As String
    Dim arreglocad_alt() As String

    Public Property Titulo() As String
        Get
            Return lblTitle.Text
        End Get
        Set(ByVal Value As String)
            lblTitle.Text = Value
        End Set
    End Property

    Public Property Idioma() As Integer
        Get
            Return viewstate("IDIOMA")
        End Get
        Set(ByVal Value As Integer)
            viewstate("IDIOMA") = Value
        End Set
    End Property

    Public Property GrupoMax() As Integer
        Get
            If IsNothing(viewstate("GRUPOMAX")) Then viewstate("GRUPOMAX") = 5
            Return viewstate("GRUPOMAX")
        End Get
        Set(ByVal Value As Integer)
            viewstate("GRUPOMAX") = Value
        End Set
    End Property

    Public Property Grupo() As Integer
        Get
            Return viewstate("GRUPO")
        End Get
        Set(ByVal Value As Integer)
            viewstate("GRUPO") = Value
        End Set
    End Property

    Public Property idElemento() As Integer
        Get
            Return viewstate("IDELEMENTO")
        End Get
        Set(ByVal Value As Integer)
            viewstate("IDELEMENTO") = Value
        End Set
    End Property

    Public Enum TipoGrupo
        Check = 0
        Radio = 1
    End Enum

    Public Property Tipo() As TipoGrupo
        Get
            Return viewstate("TIPO")
        End Get
        Set(ByVal Value As TipoGrupo)
            viewstate("TIPO") = Value
        End Set
    End Property

    Public Event Valores(ByVal Valor As String, ByVal Valoralt As String)

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsNothing(viewstate("IDIOMA")) And Not IsNothing(viewstate("GRUPO")) And Not IsNothing(viewstate("TIPO")) Then
            Call CargaDatos(CInt(viewstate("GRUPO")), CInt(viewstate("IDIOMA")), CInt(viewstate("TIPO")))
        End If
    End Sub



    Public Sub GuardaScript()
        Dim str As StringBuilder = New StringBuilder("<script>")
        str.Append(" var total=0;")
        str.Append(" var maximo=" & Me.GrupoMax & ";")
        str.Append(" function OnChecked(nombre,combo)")
        str.Append(" {  var y=document.getElementById(nombre);")
        str.Append("    if(y)")
        str.Append("    {")
        str.Append("        if(y.checked){ ")
        str.Append("        total++;")
        str.Append("        }else{total--;}")
        str.Append("        if(total>maximo){ alert('" & Me.GrupoMax & " Max.');total--;y.checked=false;}")
        str.Append("        var x=document.getElementById(combo);")
        str.Append("        if(x&&total>0){x.selectedIndex=total-1;}")
        str.Append("    }")
        str.Append(" };")
        str.Append("</script>")
        Page.RegisterStartupScript("check", str.ToString)
    End Sub

    Private Sub Checked(ByVal source As Object, ByVal e As EventArgs)
        Dim x As Integer
        x = 1
    End Sub

    Private Sub CargaDatos(ByVal grupo As Integer, ByVal idioma As Integer, ByVal Tipo As TipoGrupo)
        Dim x As Integer
        Dim datos As New clsCommonGrupoParrafos
        datos.Tables(datos.TABLA_GrupoParrafos).Columns.Add("cadena")
        datos.Tables(datos.TABLA_GrupoParrafos).Columns.Add("orden")
        datos = (New clsFacadeGrupoParrafos).GetByIdList(grupo, idioma)

        Dim datos2 As New clsCommonGrupoParrafos
        datos2.Tables(datos2.TABLA_GrupoParrafos).Columns.Add("cadena")
        datos2.Tables(datos2.TABLA_GrupoParrafos).Columns.Add("orden")
        If idioma = 2 Then
            datos2 = (New clsFacadeGrupoParrafos).GetByIdList(grupo, 1)
        Else
            datos2 = (New clsFacadeGrupoParrafos).GetByIdList(grupo, 2)
        End If

        Dim lista As DataTable
        lista = datos.Tables(datos.TABLA_GrupoParrafos)

        Dim lista2 As DataTable
        lista2 = datos2.Tables(datos.TABLA_GrupoParrafos)

        Dim pan As Panel = Me.FindControl("Panel1")
        'pan.Wrap = False
        If Not IsNothing(pan) Then
            Dim table1 As New HtmlTable
            If lista.Rows.Count > 0 And Tipo = TipoGrupo.Check Then
                Dim reng1 As New HtmlTableRow
                Dim celda11 As New HtmlTableCell
                Dim celda12 As New HtmlTableCell
                celda11.VAlign = "Top"
                celda11.Align = "right"
                Dim lab As New Label
                lab.Text = PortalCulture.GetString("A00684") & "."
                lab.CssClass = "clsDarkLabel"
                celda11.Controls.Add(lab)
                celda12.VAlign = "Top"
                reng1.Controls.Add(celda11)
                reng1.Controls.Add(celda12)
                table1.Controls.Add(reng1)
            End If
            For x = 0 To lista.Rows.Count - 1
                Dim cadena As String = lista.Rows(x).Item("cadena")
                cadena = cadena.Replace("<", "|#")
                cadena = cadena.Replace(">", "#|")

                Dim cadena2 As String = lista2.Rows(x).Item("cadena")
                cadena2 = cadena2.Replace("<", "|#")
                cadena2 = cadena2.Replace(">", "#|")

                Dim arr() As String
                arr = cadena.Split("|")
                Dim arr2() As String
                arr2 = cadena2.Split("|")


                Dim y As Integer
                Dim ListaControles As String = ""

                Dim reng As New HtmlTableRow

                Dim celda As New HtmlTableCell
                Dim celda2 As New HtmlTableCell
                celda.VAlign = "Top"
                celda2.VAlign = "Top"
                If Tipo = TipoGrupo.Radio Then
                    Dim radio As New RadioButton
                    radio.ID = "F" & x
                    radio.GroupName = "first"
                    'pan.Controls.Add(radio)
                    celda.Controls.Add(radio)
                Else
                    Dim check As New CheckBox
                    check.ID = "F" & x
                    check.Attributes.Add("onClick", "javascript:OnChecked('" & Me.ClientID & "_" & check.ID & "','" & Me.ClientID & "_C" & x & "');")
                    check.Checked = False
                    'check.AutoPostBack = True
                    'AddHandler check.CheckedChanged, AddressOf Me.Checked

                    'pan.Controls.Add(check)
                    celda.Controls.Add(check)

                    Dim combo As New DropDownList
                    combo.ID = "C" & x

                    'pan.Controls.Add(check)
                    Dim max As Integer
                    For max = 1 To Me.GrupoMax
                        combo.Items.Add(max)
                    Next
                    celda.Controls.Add(combo)

                End If
                For y = 0 To arr.Length - 1
                    If arr(y).StartsWith("#") Then
                        If arr(y).ToUpper.StartsWith("#LIST") Then
                            'Combo
                            Dim nolista As Integer

                            Try
                                nolista = CInt(Mid(arr(y), 7, (Len(arr(y)) - 7)))
                            Catch e As Exception
                                nolista = -1
                            End Try

                            If nolista >= 0 Then
                                Dim combo As New DropDownList
                                combo.ID = "combo" & x & y
                                combo.DataSource = (New clsFacadeListas).GetByNoListaIdioma(nolista, Me.Idioma)
                                combo.DataTextField = clsCommonListas.FIELD_DESCRIPCION
                                combo.DataValueField = clsCommonListas.FIELD_IDELEMENTO
                                combo.DataBind()
                                'pan.Controls.Add(combo)
                                celda2.Controls.Add(combo)
                                'ListaControles += arr(y).Replace("#", "") & "=" & combo.ID & "|"
                                ListaControles += combo.ID & "=" & combo.ID & "|"
                                Dim posins As Integer
                                posins = CStr(lista.Rows(x).Item("cadena")).IndexOf("<" & arr(y).Replace("#", "") & ">")
                                If posins <> -1 Then
                                    lista.Rows(x).Item("cadena") = CStr(lista.Rows(x).Item("cadena")).Remove(posins, arr(y).Length)
                                    lista.Rows(x).Item("cadena") = CStr(lista.Rows(x).Item("cadena")).Insert(posins, "<" & combo.ID & ">")
                                End If
                                Dim valor2 As String = ""
                                If Not IsNothing(arr2(y)) Then valor2 = arr2(y)

                                posins = CStr(lista2.Rows(x).Item("cadena")).IndexOf("<" & valor2.Replace("#", "") & ">")
                                If posins <> -1 Then
                                    lista2.Rows(x).Item("cadena") = CStr(lista2.Rows(x).Item("cadena")).Remove(posins, valor2.Length)
                                    lista2.Rows(x).Item("cadena") = CStr(lista2.Rows(x).Item("cadena")).Insert(posins, "<" & combo.ID & ">")
                                End If

                            End If
                        Else
                            'Textbox
                            Dim caja As New TextBox
                            caja.ID = "caja" & x & y
                            caja.CssClass = "TextBox"
                            caja.Text = arr(y).Replace("#", "")
                            'pan.Controls.Add(caja)
                            celda2.Controls.Add(caja)
                            'ListaControles += arr(y).Replace("#", "") & "=" & caja.ID & "|"
                            ListaControles += caja.ID & "=" & caja.ID & "|"
                            Dim posins As Integer
                            posins = CStr(lista.Rows(x).Item("cadena")).IndexOf("<" & arr(y).Replace("#", "") & ">")
                            If posins <> -1 Then
                                lista.Rows(x).Item("cadena") = CStr(lista.Rows(x).Item("cadena")).Remove(posins, arr(y).Length)
                                lista.Rows(x).Item("cadena") = CStr(lista.Rows(x).Item("cadena")).Insert(posins, "<" & caja.ID & ">")
                            End If
                            Dim valor2 As String = ""
                            If Not IsNothing(arr2(y)) Then valor2 = arr2(y)

                            posins = CStr(lista2.Rows(x).Item("cadena")).IndexOf("<" & valor2.Replace("#", "") & ">")
                            If posins <> -1 Then
                                lista2.Rows(x).Item("cadena") = CStr(lista2.Rows(x).Item("cadena")).Remove(posins, valor2.Length)
                                lista2.Rows(x).Item("cadena") = CStr(lista2.Rows(x).Item("cadena")).Insert(posins, "<" & caja.ID & ">")
                            End If

                        End If
                    Else
                        'Label
                        Dim etiq As New Label
                        etiq.ID = "label" & x & y
                        etiq.CssClass = "clslabel"
                        'pan.Controls.Add(etiq)
                        celda2.Controls.Add(etiq)
                        etiq.Text = arr(y)
                    End If
                Next
                Dim enter As New LiteralControl
                enter.Text = "<BR>"
                'pan.Controls.Add(enter)

                reng.Controls.Add(celda)
                reng.Controls.Add(celda2)
                table1.Controls.Add(reng)

                ReDim Preserve arreglo(x)
                arreglo(x) = ListaControles
                ReDim Preserve arreglocad(x)
                arreglocad(x) = lista.Rows(x).Item("cadena")
                ReDim Preserve arreglocad_alt(x)
                arreglocad_alt(x) = lista2.Rows(x).Item("cadena")
            Next
            pan.Controls.Add(table1)
        End If

        viewstate("ARREGLO") = arreglo
        viewstate("ARREGLOCAD") = arreglocad
        viewstate("ARREGLOCADalt") = arreglocad_alt
    End Sub

    Public Function Valor() As String
        Dim controlp As Panel = Me.FindControl("Panel1")
        Dim rb As Control
        Dim cadenaprinc As String = ""
        Dim cadenaprinc_alt As String = ""
        Dim lista As New DropDownList
        Dim listaAlt As New DropDownList

        Dim tabla As HtmlTable = controlp.Controls(0)
        Dim xx As Integer
        For xx = 0 To tabla.Controls.Count - 1 'renglones
            Dim reng As HtmlTableRow = tabla.Controls(xx)
            Dim celda1 As HtmlTableCell = reng.Controls(0) 'celda de seleccion
            Dim control As HtmlTableCell = reng.Controls(1) 'celda con el contenido

            rb = celda1.Controls(0)
            If rb.GetType.ToString.ToUpper = UCase("system.Web.UI.WebControls.RadioButton") Then
                Dim rad As RadioButton = CType(rb, RadioButton)
                If rad.Checked Then
                    Dim pos As Integer = CInt(Mid(rad.ID, 2, rad.ID.Length - 1))
                    Dim cadena As String = arreglocad(pos)
                    Dim cadena_alt As String = arreglocad_alt(pos)
                    Dim controles As String = arreglo(pos)
                    Dim arr() As String

                    arr = controles.Split("|")
                    Dim x As Integer
                    For x = 0 To arr.Length - 2
                        Dim datos() As String
                        datos = arr(x).Split("=")
                        If datos(0).ToUpper.StartsWith("COMBO") Then
                            Dim controlsel As DropDownList = control.FindControl(datos(1))
                            cadena = cadena.Replace("<" & datos(0) & ">", controlsel.SelectedItem.Text)

                            Dim lista_alt As clsCommonListas
                            If Me.Idioma = 2 Then
                                lista_alt = (New clsFacadeListas).GetByIdElemento(controlsel.SelectedValue, 1)
                            Else
                                lista_alt = (New clsFacadeListas).GetByIdElemento(controlsel.SelectedValue, 2)
                            End If
                            cadena_alt = cadena_alt.Replace("<" & datos(0) & ">", lista_alt.Tables(lista_alt.TABLA_LISTAS).Rows(0).Item(lista_alt.FIELD_DESCRIPCION))
                        Else
                            Dim controlsel As TextBox = control.FindControl(datos(1))
                            cadena = cadena.Replace("<" & datos(0) & ">", controlsel.text)
                            cadena_alt = cadena_alt.Replace("<" & datos(0) & ">", controlsel.text)
                        End If
                    Next
                    cadenaprinc += cadena & vbCrLf
                    cadenaprinc_alt += cadena_alt & vbCrLf
                End If
            ElseIf rb.GetType.ToString.ToUpper = UCase("system.Web.UI.WebControls.CheckBox") Then
                Dim rad As CheckBox = CType(rb, CheckBox)

                If rad.Checked Then
                    Dim pos As Integer = CInt(Mid(rad.ID, 2, rad.ID.Length - 1))
                    Dim cadena As String = arreglocad(pos)
                    Dim cadena_alt As String = arreglocad_alt(pos)
                    Dim controles As String = arreglo(pos)
                    Dim arr() As String

                    arr = controles.Split("|")
                    Dim x As Integer
                    For x = 0 To arr.Length - 2
                        Dim datos() As String
                        datos = arr(x).Split("=")
                        If datos(0).ToUpper.StartsWith("COMBO") Then
                            Dim controlsel As DropDownList = control.FindControl(datos(1))
                            cadena = cadena.Replace("<" & datos(0) & ">", controlsel.SelectedItem.Text)

                            Dim lista_alt As clsCommonListas
                            If Me.Idioma = 2 Then
                                lista_alt = (New clsFacadeListas).GetByIdElemento(controlsel.SelectedValue, 1)
                            Else
                                lista_alt = (New clsFacadeListas).GetByIdElemento(controlsel.SelectedValue, 2)
                            End If
                            cadena_alt = cadena_alt.Replace("<" & datos(0) & ">", lista_alt.Tables(lista_alt.TABLA_LISTAS).Rows(0).Item(lista_alt.FIELD_DESCRIPCION))

                        Else
                            Dim controlsel As TextBox = control.FindControl(datos(1))
                            cadena = cadena.Replace("<" & datos(0) & ">", controlsel.text)
                            cadena_alt = cadena_alt.Replace("<" & datos(0) & ">", controlsel.text)
                        End If
                    Next
                    cadenaprinc += cadena & vbCrLf
                    cadenaprinc_alt += cadena_alt & vbCrLf
                    'obtener el orden del dropdownlist-------------------------------
                    Dim combo As DropDownList = control.FindControl("C" + Mid(rad.ID, 2, rad.ID.Length))
                    Dim orden As Integer = 1
                    If Not IsNothing(combo) Then orden = combo.SelectedValue
                    'If orden > lista.Items.Count Then orden = lista.Items.Count
                    lista.Items.Add(cadena)
                    lista.Items(lista.Items.Count - 1).Value = orden
                    listaAlt.Items.Add(cadena_alt)
                    listaAlt.Items(listaAlt.Items.Count - 1).Value = orden
                    '------------------------------------------------------------------
                End If
                rad.Checked = False
            End If
        Next
        Valor = cadenaprinc
        If lista.Items.Count > 0 Then
            Dim x As Integer

            Dim listat As New DropDownList
            Dim listatalt As New DropDownList
            Dim cont As Integer
            For cont = 1 To Me.GrupoMax
                For x = 0 To lista.Items.Count - 1
                    If lista.Items(x).Value = cont Then
                        listat.Items.Add(lista.Items(x).Text)
                        listatalt.Items.Add(listaAlt.Items(x).Text)
                    End If
                Next
            Next
            cadenaprinc = ""
            cadenaprinc_alt = ""
            For x = 0 To listat.Items.Count - 1
                cadenaprinc += listat.Items(x).Text & vbCrLf
                cadenaprinc_alt += listatalt.Items(x).Text & vbCrLf
            Next
        End If
        RaiseEvent Valores(cadenaprinc, cadenaprinc_alt)
    End Function


End Class
