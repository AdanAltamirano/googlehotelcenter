Public Class IDSMapping
    Inherits PaginaBase 'System.Web.UI.Page

    Dim RateCrsHotel() As DataRow 'Tabla de BD OzRateCRS
    Dim RateCrsHotelId As Integer 'Relacionado con Conflux..IDS_Hotels.Hotel_IDS
    Dim UnmappedRoomsAndRatesPlan As DataSet
    Dim strError As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsHotelSelected Then
            MyBase.redirectTo(pages.SearchHotel)
        End If

        If HasIDSConfig(cInfoActual.Empresa) Then
            Dim IsUpdatedMappedIDS As Boolean = False
            UnmappedRoomsAndRatesPlan = GetUnMappedRoomsRatePlans(RateCrsHotelId)

            If Not UnmappedRoomsAndRatesPlan Is Nothing AndAlso UnmappedRoomsAndRatesPlan.Tables.Count > 0 AndAlso (UnmappedRoomsAndRatesPlan.Tables(0).Rows.Count > 0 Or UnmappedRoomsAndRatesPlan.Tables(1).Rows.Count > 0) Then
                CheckMappCodes(IsUpdatedMappedIDS)

                If IsUpdatedMappedIDS Then
                    UnmappedRoomsAndRatesPlan = GetUnMappedRoomsRatePlans(RateCrsHotelId)
                End If

                LoadUnmappedRoomsAndRatesPlan(UnmappedRoomsAndRatesPlan)
            Else
                lblError.Text = "No hay registros"
            End If
        End If
    End Sub

    Private Function HasIDSConfig(ByVal idEmpresaUV As Int32) As Boolean
        'Ravia si el hotel existe en OzRateCRS..Hoteles
        Dim RateCrsHotels As DataSet
        RateCrsHotels = GetRateCrsHotels()
        If Not RateCrsHotels Is Nothing And RateCrsHotels.Tables(0).Rows.Count > 0 Then
            RateCrsHotel = RateCrsHotels.Tables(0).Select("code = '" & idEmpresaUV.ToString & "'")
            If RateCrsHotel.Length > 0 Then
                RateCrsHotelId = RateCrsHotel(0).Item("idHotel")

                Return True
            End If
        End If

        Return False
    End Function

    Private Function GetRateCrsHotels() As DataSet
        Try
            With New ConfluxMapping.clsDataAccess(System.Configuration.ConfigurationManager.AppSettings("RateCrsConnectionString"))
                Return .GetRateCrsHotels(strError)
            End With
        Catch ex As Exception
            lblError.Text = ex.Message
            Return Nothing
        End Try
    End Function

    Private Function GetUnMappedRoomsRatePlans(ByVal IDS_Hotel As Integer) As DataSet
        Try
            With New ConfluxMapping.clsDataAccess(System.Configuration.ConfigurationManager.AppSettings("ConfluxConnectionString"))
                Return .GetUnmappedRoomsRateplansByHotel_IDS(IDS_Hotel, strError)
            End With
        Catch ex As Exception
            lblError.Text = ex.Message
            Return Nothing
        End Try

    End Function

    Private Sub CheckMappCodes(ByRef IsUpdatedMappedIDS As Boolean)
        Dim RateCrsRoom As DataSet
        Dim RateCrsRatePlan As DataSet
        Try

            For Each Room As DataRow In UnmappedRoomsAndRatesPlan.Tables(0).Rows
                With New ConfluxMapping.clsDataAccess(System.Configuration.ConfigurationManager.AppSettings("RateCrsConnectionString"))
                    RateCrsRoom = .GetUvRoomOrRateCodes(Room.Item("ChannelIdRateCRS"), Room.Item("RoomSourceId").ToString.Trim, RateCrsHotelId, ConfluxMapping.clsData.CodeType.Rooms, strError)
                    If Not RateCrsRoom Is Nothing AndAlso RateCrsRoom.Tables.Count > 0 AndAlso RateCrsRoom.Tables(0).Rows.Count > 0 Then
                        With New ConfluxMapping.clsDataAccess(System.Configuration.ConfigurationManager.AppSettings("ConfluxConnectionString"))
                            If .UpdateMappedIDS(RateCrsHotelId, Room.Item("ChannelId"), Room.Item("RoomSourceId").ToString.Trim) Then
                                IsUpdatedMappedIDS = True
                            End If
                        End With
                    End If
                End With
            Next

            For Each Rate As DataRow In UnmappedRoomsAndRatesPlan.Tables(1).Rows
                With New ConfluxMapping.clsDataAccess(System.Configuration.ConfigurationManager.AppSettings("RateCrsConnectionString"))

                    Dim CRSIDSHotel As DataSet = .GetIDSCrsHotels(RateCrsHotelId, Rate.Item("ChannelIdRateCRS"), strError)
                    With New ConfluxMapping.clsDataAccess(System.Configuration.ConfigurationManager.AppSettings("RateCrsConnectionString"))
                        RateCrsRatePlan = .GetUvRoomOrRateCodes(Rate.Item("ChannelIdRateCRS"), Rate.Item("RateSourceId").ToString.Trim, CRSIDSHotel.Tables(0).Rows(0).Item("codigo"), ConfluxMapping.clsData.CodeType.Rates, strError)
                    End With

                    If Not RateCrsRatePlan Is Nothing AndAlso RateCrsRatePlan.Tables.Count > 0 AndAlso RateCrsRatePlan.Tables(0).Rows.Count > 0 Then
                        With New ConfluxMapping.clsDataAccess(System.Configuration.ConfigurationManager.AppSettings("ConfluxConnectionString"))
                            If .UpdateMappedIDS(RateCrsHotelId, Rate.Item("ChannelId"), "", Rate.Item("RateSourceId").ToString.Trim, strError) Then
                                IsUpdatedMappedIDS = True
                            End If
                        End With
                    End If
                End With
            Next
            If Not String.IsNullOrEmpty(strError) Then
                lblError.Text = strError
                strError = String.Empty
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub LoadUnmappedRoomsAndRatesPlan(ByVal UnmappedRoomsAndRatesPlan As DataSet)
        gvRatesPlan.DataSource = UnmappedRoomsAndRatesPlan.Tables(1)
        gvRatesPlan.DataBind()

        gvRooms.DataSource = UnmappedRoomsAndRatesPlan.Tables(0)
        gvRooms.DataBind()
    End Sub

    Protected Sub gvRatesPlan_RowDataBound(ByVal sendar As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRatesPlan.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then

        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkEliminar As LinkButton = e.Row.Cells(4).FindControl("btnEliminar")
            Dim btnEdit As LinkButton = e.Row.Cells(4).FindControl("btnEdit")
            Dim idIDS As Integer = CInt(e.Row.Cells(0).Text)

            If lnkEliminar IsNot Nothing Then
                lnkEliminar.Attributes("onclick") = "if(!confirm('" & PortalCulture.GetString("01839") & "')) return false;"
            End If

            If btnEdit IsNot Nothing Then
                btnEdit.Attributes("style") = "display:block;"
                btnEdit.Attributes("style") = "display:block;"
            End If
        End If

    End Sub

    Protected Sub gvRooms_RowDataBound(ByVal sendar As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRooms.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then

        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkEliminar As LinkButton = e.Row.Cells(4).FindControl("btnEliminar")
            Dim btnEdit As LinkButton = e.Row.Cells(4).FindControl("btnEdit")
            Dim idIDS As Integer = CInt(e.Row.Cells(0).Text)

            If lnkEliminar IsNot Nothing Then
                lnkEliminar.Attributes("onclick") = "if(!confirm('" & PortalCulture.GetString("01839") & "')) return false;"
            End If

            If btnEdit IsNot Nothing Then
                btnEdit.Attributes("style") = "display:block;"
                btnEdit.Attributes("style") = "display:block;"
            End If
        End If

    End Sub

    Protected Sub gvRatesPlan_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvRatesPlan.EditIndex = e.NewEditIndex
        LoadUnmappedRoomsAndRatesPlan(UnmappedRoomsAndRatesPlan)
    End Sub
    Protected Sub gvRatesPlan_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

    End Sub

    Protected Sub gvRatesPlan_RowCancelEditing(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        gvRatesPlan.EditIndex = -1
        LoadUnmappedRoomsAndRatesPlan(UnmappedRoomsAndRatesPlan)
    End Sub

    Protected Sub gvRooms_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvRooms.EditIndex = e.NewEditIndex
        LoadUnmappedRoomsAndRatesPlan(UnmappedRoomsAndRatesPlan)
    End Sub
    Protected Sub gvRooms_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

    End Sub

    Protected Sub gvRooms_RowCancelEditing(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        gvRooms.EditIndex = -1
        LoadUnmappedRoomsAndRatesPlan(UnmappedRoomsAndRatesPlan)
    End Sub
End Class