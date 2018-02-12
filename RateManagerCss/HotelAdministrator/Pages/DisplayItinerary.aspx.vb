
Partial Public Class DisplayItinerary
    Inherits PaginaBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        loadPackage()
    End Sub
   
    Public Function loadPackage() As DataRow
        Dim HUSDtotal As Double = 0
        Dim ACTDtotal As Double = 0
        Dim CUSDtotal As Double = 0
        Dim FUSDtotal As Double = 0
        Dim USDtotal As Double
        Dim TotalPenaltyAmount As Double = 0
        If Not Request.QueryString("itinerary") Is Nothing AndAlso Request.QueryString("itinerary") <> "" Then
            Dim ds As DataSet
            With New PortalLibraries.DAPackages
                ds = .GetPackagebyItinerary(Request.QueryString("itinerary"))
            End With
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                'lblItiNoItinerary.Text = Request.QueryString("itinerary")
                lblItiNoItinerary.Text = ds.Tables(0).Rows(0)("Itinerary").ToString
                With ds.Tables(0).Rows(0)
                    
                    If Not .IsNull("HotelReservationId") Then
                        CtrlHotelItinerary1.getData(.Item("HotelReservationId"))
                        If CtrlHotelItinerary1.PenaltyAmount > 0 Then
                            TotalPenaltyAmount += CtrlHotelItinerary1.PenaltyAmount
                        End If
                    Else
                        CtrlHotelItinerary1.Visible = False
                    End If
                    If Not .IsNull("ActivityReservationId") Then

                        CtrlActivityItinerary1.FillReservation(Request.QueryString("itinerary"))
                        If CtrlActivityItinerary1.PenaltyAmount > 0 Then
                            TotalPenaltyAmount += CtrlActivityItinerary1.PenaltyAmount
                        End If
                        '   ACTDtotal = Convert.ToDouble(CtrlsummaryActivity1.TotalReservacion)
                    Else
                        CtrlActivityItinerary1.Visible = False
                    End If

                    If TotalPenaltyAmount > 0 Then
                        lblCancelDeadLine.Visible = True
                        lblEPenaltyAmount.Visible = True
                        lblPenaltyAmount.Visible = True

                        lblPenaltyAmount.InnerText = Format(TotalPenaltyAmount, "##,##0.00")
                    End If

                    lbCancelationNumber.Visible = False
                    lblCancelationNumber.Visible = False
                    'If .Item("status") = 3 Then
                    '    btnCancelItinerary.Visible = False
                    '    lbCancelationNumber.Visible = True
                    '    lblCancelationNumber.Visible = True
                    '    lblCancelationNumber.InnerHtml = .Item("NoCancellation")
                    '    lblTitle.InnerHtml = String.Format(PackagesLanguage.GetString("PACKAGES000489"), (New PortalPartnersCfg).Name)
                    'Else
                    '    lblTitle.InnerHtml = String.Format(PackagesLanguage.GetString("PACKAGES000377"), (New PortalPartnersCfg).Name)
                    'End If
                    'If Not Request.QueryString("Page") Is Nothing Then
                    '    btnCancelItinerary.Visible = False
                    'End If
                    
                End With


                Return ds.Tables(0).Rows(0)
            Else
                'Response.Redirect(MyBase.SecureSite & "/Portal/Secure/Traveler/MyTrips.aspx")
            End If
        End If
        Return Nothing

    End Function

    Private Sub DisplayItinerary_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.lblConfirm.Text = PortalCulture.GetString("M000348")
        Me.lblItinerary.Text = PortalCulture.GetString("M0UT02717", True)
    End Sub
End Class