Imports System.IO
Partial Public Class ctrlMenuUl
    Inherits UserControlBase

    Enum MenuType
        SoloPrimerNivel
        SoloSegundoNivel
    End Enum


    Public ReadOnly Property HaveDefaultTarget() As Boolean
        Get
            Return (DefaultTarget.Trim <> String.Empty)
        End Get
    End Property


    Public Property DefaultTarget() As String
        Get
            If ViewState(String.Format("{0}DefaultTarget", Me.ClientID)) Is Nothing Then
                Return ""
            End If
            Return CType(ViewState(String.Format("{0}DefaultTarget", Me.ClientID)), String)
        End Get
        Set(ByVal value As String)
            ViewState(String.Format("{0}DefaultTarget", Me.ClientID)) = value
        End Set
    End Property



    Public Property CurrentMenuType() As MenuType
        Get
            If ViewState(String.Format("{0}CurrentMenuType", Me.ClientID)) Is Nothing Then
                Return MenuType.SoloPrimerNivel
            End If
            Return CType(ViewState(String.Format("{0}CurrentMenuType", Me.ClientID)), MenuType)
        End Get
        Set(ByVal value As MenuType)
            ViewState(String.Format("{0}CurrentMenuType", Me.ClientID)) = value
        End Set
    End Property

    Public Property CurrentRoles() As String
        Get
            If ViewState(String.Format("{0}CurrentRoles", Me.ClientID)) Is Nothing Then
                Return ""
            End If
            Return CType(ViewState(String.Format("{0}CurrentRoles", Me.ClientID)), String)
        End Get
        Set(ByVal value As String)
            ViewState(String.Format("{0}CurrentRoles", Me.ClientID)) = value
        End Set
    End Property



    Public Sub LoadMenu(ByVal xmlMenu As String)
        litMenu.Text = String.Empty
        If (xmlMenu.Trim() <> String.Empty) Then
            Dim ds As New DataSet
            Dim result As String = String.Empty
            Dim itemNumber As Integer = 0
            Dim userRoles As String = CurrentRoles


            ds.ReadXml(New System.Xml.XmlTextReader(New System.IO.StringReader(xmlMenu)))
            If (ds.Tables("menuItem").Rows.Count > 0) Then
                BuilMenu(-1, 1, ds, userRoles, itemNumber, result)
                Select Case CurrentMenuType
                    Case MenuType.SoloPrimerNivel

                        result = String.Format("<ul class=""navbar-nav bd-navbar-nav flex-row"">{0}</ul>", result)
                    Case MenuType.SoloSegundoNivel
                        result = result
                End Select
                litMenu.Text = result
            End If
        End If
    End Sub

    Private Sub BuilMenu(ByVal idPadre As Integer, ByVal nivel As Integer, ByVal ds As DataSet, ByVal userRoles As String, ByRef itemNumber As Integer, ByRef result As String)

        Dim countItems As Integer = 0
        Dim firstsubmenu = True
        For Each dr As DataRow In ds.Tables("menuItem").Rows
            Dim text As String = Nothing
            Dim roles As String = Nothing
            Dim url As String = Nothing
            Dim ico As String = Nothing
            Dim menuItem_Id As Integer = -1
            Dim subMenu_Id As Integer = -1
            Dim nextIdPadre As Integer = -2
            'Dim ul As String = "<ul class='submenuprin'><li><a>Prueba 1</a></li><li><a>Prueba 2</a></li><li><a>Prueba 3</a></li></ul>"

            If (dr.Table.Columns.IndexOf("text")) <> -1 AndAlso Not dr.IsNull("text") Then text = dr("text")
            If (dr.Table.Columns.IndexOf("roles")) <> -1 AndAlso Not dr.IsNull("roles") Then roles = dr("roles")
            If (dr.Table.Columns.IndexOf("url")) <> -1 AndAlso Not dr.IsNull("url") Then url = dr("url")
            If (dr.Table.Columns.IndexOf("ico")) <> -1 AndAlso Not dr.IsNull("ico") Then ico = dr("ico")
            If (dr.Table.Columns.IndexOf("menuItem_Id")) <> -1 AndAlso Not dr.IsNull("menuItem_Id") Then menuItem_Id = dr("menuItem_Id")
            If (dr.Table.Columns.IndexOf("subMenu_Id")) <> -1 AndAlso Not dr.IsNull("subMenu_Id") Then subMenu_Id = dr("subMenu_Id")

            If idPadre = subMenu_Id Then
                If CheckRole(userRoles, roles) Then
                    nextIdPadre = GetPadre(menuItem_Id, ds)
                    If nivel = 1 Then
                        Select Case CurrentMenuType
                            Case MenuType.SoloPrimerNivel
                                If nextIdPadre <> -2 Then

                                    Dim html As String = String.Empty
                                    html &= "{0}<li class=""nav-item"" id=""{1}_{2}_li"">"
                                    html &= "<a class=""nav-link"" data-toggle=""dropdown"" aria-expanded=""false"" aria-haspopup=""true"" id=""x_" & nivel & "_" & menuItem_Id & """ {3}>{4}</a>"
                                    html &= "<div aria-labelledby=""x_" & nivel & "_" & menuItem_Id & """ class=""dropdown-menu"">"


                                    'result += String.Format("{0}<li class=""nav-item"" id = '{1}_{2}_li'><a class=""nav-link dropdown-toggle"" data-toggle=""dropdown"" aria-expanded=""false"" aria-haspopup=""true"" id=""x_" & nivel & "_" & menuItem_Id & """ {3}>{4}</a><ul aria-labelledby=""x_" & nivel & "_" & menuItem_Id & """ class='dropdown-menu dropdown-menu-right'>", GetTabs(nivel), nivel, menuItem_Id, GetHrefLeve1(url, nivel, menuItem_Id), GetString(text).ToUpper)
                                    result += String.Format(html, GetTabs(nivel), nivel, menuItem_Id, GetHrefLeve1(url, nivel, menuItem_Id), GetString(text).ToUpper)
                                    BuilMenu(nextIdPadre, nivel + 1, ds, userRoles, itemNumber, result)
                                    result += String.Format("</div></li>{0}", vbCrLf)
                                    itemNumber += 1
                                Else
                                    result += String.Format("{0}<li class=""nav-item"" id = '{1}_{2}_li'><a class=""nav-link"" {3}>{4}</a></li>", GetTabs(nivel), nivel, menuItem_Id, GetHrefLeve1(url, nivel, menuItem_Id), GetString(text).ToUpper)
                                End If

                            Case MenuType.SoloSegundoNivel
                                result += String.Format("<ul class='dvhide list-unstyled components' id={0}_{1}_div>{2}", nivel, menuItem_Id, vbCrLf)
                                'result += "<ul class='submenu'>"
                                nextIdPadre = GetPadre(menuItem_Id, ds)
                                BuilMenu(nextIdPadre, nivel + 1, ds, userRoles, itemNumber, result)
                                'result += "<ul/>"
                                result += String.Format("</ul>{0}", vbCrLf)

                        End Select
                    Else
                        itemNumber += 1
                        If firstsubmenu Then
                            firstsubmenu = False
                            countItems = 1
                        End If

                        Select Case CurrentMenuType
                            Case MenuType.SoloPrimerNivel
                                If nivel = 2 Then
                                    If countItems = (ds.Tables("menuItem").Select("subMenu_Id = " & subMenu_Id).Count) Then
                                        result += String.Format("{0}<a class=""dropdown-item"" href='{1}' {2}>{3}</a>{4}", GetTabs(nivel), GetUrl(url), GetTarget(url), GetString(text), vbCrLf)
                                    Else
                                        result += String.Format("{0}<a class=""dropdown-item"" href='{1}' {2}>{3}</a><div class=""dropdown-divider""></div>{4}", GetTabs(nivel), GetUrl(url), GetTarget(url), GetString(text), vbCrLf)
                                    End If


                                End If

                            Case MenuType.SoloSegundoNivel
                                If nivel = 2 Then
                                    result += String.Format("{0}<li><a href='{1}' {2}>{3}</a></li><div class=""dropdown-divider""></div>{4}", GetTabs(nivel), GetUrl(url), GetTarget(url), GetString(text), vbCrLf)
                                Else
                                    result += String.Format("{0}<li><a href='{1}' {2}><span class='SubMenuLevel3'>{3}</span></a></li><div class=""dropdown-divider""></div>{4}", GetTabs(nivel), GetUrl(url), GetTarget(url), GetString(text), vbCrLf)
                                End If
                        End Select
                        If nextIdPadre <> -2 Then
                            BuilMenu(nextIdPadre, nivel + 1, ds, userRoles, itemNumber, result)
                        End If
                    End If
                End If
            End If
            countItems += 1
        Next
    End Sub
    Private Function GetUrl(ByVal url As String) As String
        If (Not url Is Nothing AndAlso url.Trim() <> String.Empty) Then
            'Dim AplicationUrl = String.Format("{0}://{1}{2}/", Request.Url.Scheme, Request.Url.Authority, url.Trim())
            Dim AplicationUrl = String.Format("{0}", url.Trim())
            Return AplicationUrl
        End If
        Return "#"
    End Function


    Private Function GetHrefLeve1(ByVal url As String, ByVal nivel As Integer, ByVal menuItem_Id As Integer) As String
        If (url Is Nothing) Then
            Return String.Format(" href='#{0}_{1}' ", nivel, menuItem_Id)
        Else
            Return String.Format(" href='{0}' {1} ", url, GetTarget(url))
        End If
        Return ""
    End Function
    Private Function GetTarget(ByVal url As String) As String

        If Not url Is Nothing AndAlso url.Trim <> String.Empty AndAlso HaveDefaultTarget Then
            Return String.Format(" target='{0}' ", DefaultTarget)
        End If
        Return ""
    End Function
    Private Function GetPadre(ByVal id, ByVal ds)
        If Not ds.Tables("subMenu") Is Nothing Then
            If (ds.Tables("subMenu").Rows.Count > 0) Then
                For Each dr As DataRow In ds.Tables("subMenu").Rows
                    If dr("menuItem_Id") = id Then
                        Return dr("subMenu_Id")
                    End If
                Next
            End If
        End If
        Return -2
    End Function
    Private Function GetTabs(ByVal level) As String
        Dim ca As String = ""
        For i As Integer = 0 To level
            ca += vbTab
        Next
        Return ca


    End Function
    Private Function GetString(ByVal s As String) As String
        If (s IsNot Nothing) Then
            Return s.Trim
        End If
        Return ""
    End Function
    Private Function CheckRole(ByVal rs As String, ByVal rm As String) As Boolean
        If rm Is Nothing Then
            Return True
        End If

        For Each s1 As String In rs.Split(",")
            If s1.Trim() <> String.Empty Then
                For Each s2 As String In rm.Split(",")
                    If s2.Trim() <> String.Empty Then
                        If s1.ToUpper().Trim = s2.ToUpper.Trim Then
                            Return True
                        End If
                    End If
                Next
            End If
        Next

        Return False
    End Function


End Class