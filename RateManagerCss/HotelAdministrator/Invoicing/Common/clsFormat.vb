Imports Oz.UniBilling.Common.Hotels


Public Class clsFormat

    Public Shared Function GetDisplayName(ByVal status As CommissionDetailStatus) As String
        Dim displayName As String = String.Empty

        Select Case status
            Case CommissionDetailStatus.OK
                displayName = PortalCulture.GetString("M0BT0000051")
            Case CommissionDetailStatus.NoShow
                displayName = PortalCulture.GetString("M0BT0000052")
            Case CommissionDetailStatus.ModifyNights
                displayName = PortalCulture.GetString("M0BT0000053")
            Case CommissionDetailStatus.CancelledReservation
                displayName = PortalCulture.GetString("M0BT0000061")
            Case CommissionDetailStatus.Duplicated
                displayName = PortalCulture.GetString("M0BT0000304")
            Case CommissionDetailStatus.Adjusted
                displayName = PortalCulture.GetString("M000665")
            Case Else
                displayName = PortalCulture.GetString("M0BT0000323")
        End Select

        Return displayName
    End Function

    Public Shared Function GetDisplayName(ByVal transaction As BillingStatementTransaction) As String
        Dim displayName As String = String.Empty

        Select Case transaction
            Case BillingStatementTransaction.PortalInitialSetup
                displayName = PortalCulture.GetString("M0BT0000316")
            Case BillingStatementTransaction.UniPantallaInitialSetup
                displayName = PortalCulture.GetString("M0BT0000317")
            Case BillingStatementTransaction.GdsInitialSetup
                displayName = PortalCulture.GetString("M0BT0000318")
            Case BillingStatementTransaction.PortalAnnualSuscription
                displayName = PortalCulture.GetString("M0BT0000319")
            Case BillingStatementTransaction.UniPantallaAnnualSuscription
                displayName = PortalCulture.GetString("M0BT0000320")
            Case BillingStatementTransaction.GdsAnnualSuscription
                displayName = PortalCulture.GetString("M0BT0000321")
            Case BillingStatementTransaction.ReservationCommission
                displayName = PortalCulture.GetString("M0BT0000322")
            Case BillingStatementTransaction.WebSiteModification
                displayName = PortalCulture.GetString("M0BT0000346")
        End Select

        Return displayName
    End Function

    Public Shared Function GetDisplayName(ByVal rateType As RateType) As String
        Dim displayName As String = String.Empty

        Select Case rateType
            Case rateType.Commissionable
                displayName = PortalCulture.GetString("M0BT0000109")
        End Select

        Return displayName
    End Function

    Public Shared Function GetDisplayName(ByVal status As Oz.BillingSystem.Common.BillingStatementStatus) As String
        Dim displayName As String = String.Empty

        Select Case status
            Case Oz.BillingSystem.Common.BillingStatementStatus.Generated
                displayName = PortalCulture.GetString("M0BT0000324")
            Case Oz.BillingSystem.Common.BillingStatementStatus.Reconciled
                displayName = PortalCulture.GetString("M0BT0000325")
            Case Oz.BillingSystem.Common.BillingStatementStatus.Paid
                displayName = PortalCulture.GetString("M0BT0000326")
            Case Else
                displayName = PortalCulture.GetString("M0BT0000327")
        End Select

        Return displayName
    End Function

End Class

