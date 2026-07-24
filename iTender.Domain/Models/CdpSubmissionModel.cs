namespace iTender.Domain.Models
{
    public class CdpSubmissionModel
    {
        public Guid Id { get; set; }
        public Guid? ContractorId { get; set; }
        public Guid? CdpSubmissionId { get; set; }
        public decimal? CdpValueZar { get; set; }
        public int? CompetenceRatingAtEntry { get; set; }
        public int? CompetenceRatingAtExit { get; set; }
        public int? CompetencyAssessmentPerformed { get; set; }
        public DateTime? DateOfSubmission { get; set; }
        public bool? EmployerApproved { get; set; }
        public string EmployerComments { get; set; }
        public bool? EmployerRejected { get; set; }
        public bool? EmployerSubmitted { get; set; }
        public string FinalResult { get; set; }
        public string GapAnalysisAtEntry { get; set; }
        public bool? InductionCompleted { get; set; }
        public bool? IsCurrent { get; set; }
        public string LiteracyNumeracyAssessmentAtEntry { get; set; }
        public string Name { get; set; }
        public string OverallCompetenceAssessmentAtEntry { get; set; }
        public string OverallCompetenceAssessmentAtExit { get; set; }
        public string ProgressComments { get; set; }
        public string ReasonForNotApplying { get; set; }
        public bool? RemovedFromCdp { get; set; }
        public string RplAssessmentAtEntry { get; set; }
        public string TargetClassOfWork { get; set; }
        public string TargetGrade { get; set; }
        public bool? TrainingCompleted { get; set; }
        public string TrainingRequirements { get; set; }
        public int? StateCode { get; set; }
        public int? StatusCode { get; set; }
    }
}
