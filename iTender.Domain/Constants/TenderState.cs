namespace iTender.Domain.Constants
{
    public class TenderState
    {
        public const string DRAFT_STATUS = "1";
        public const string ADVERTISED_STATUS = "100000000";
        public const string CANCELLED_STATUS = "100000001";
        public const string CLOSED_STATUS = "100000002";
        public const string EOI_CRM_VALUE = "100000000";
        public const string TENDER_CRM_VALUE = "100000001";
        public const string APPROVED_CHANGEREQUEST_STATUS = "100000003";
        public const string CANCELLED_CHANGEREQUEST_STATUS = "100000005";
        public const string DECLINED_CHANGEREQUEST_STATUS = "100000004";
        public const string PENDING_CHANGEREQUEST_STATUS = "100000002";
        public const string REQUESTED_CHANGEREQUEST_STATUS = "100000001";
    }
}
