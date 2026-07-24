using System;
using System.Collections.Generic;
using System.Text;

namespace iTender.Domain.Enums
{
    public enum ApplicationType
    {
        NewApplication = 100000000,
        Upgrade = 100000001,
        Additions = 100000002,
        AnnualUpdate = 100000003,
        ThreeYearRenewal = 100000004,
        UpdateContactDetails = 100000005,
        ReRegistration = 100000006,
        Downgrade = 100000007,
        Cancellation = 100000009,
        UpdateEnterpriseParticulars = 100000010,
        UpdatePrincipalsOwnership = 100000011,
        UpdatePEStatus = 100000012,
        UpdateBBBEEDetails = 100000013,
        UpdateTCCRenewal = 100000014,
        UpdateElectricalLicenceRenewal = 100000015,
        UpdateCapturePayment = 100000016
    }

    public enum ApplicationStatus
    {
        Draft = 1,
        Submitted = 100000000,
        InProgress = 100000001,
        Approved = 100000002,
        Rejected = 100000003,
        Cancelled = 100000004
    }
}
