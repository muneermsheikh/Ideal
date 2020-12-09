export interface IEnquiry {
    id: number;
    customerId: number;
    enquiryRef: string;
    enquiryNo: string;
    enquiryDate: string;
    basketId: string;
    completeBy: string;
    readyToReview: string;
    reviewStatus: string;
    reviewedById: number;
    reviewedOn: string;
    enquiryStatus: string;
    projectManagerId: number;
    accountExecutiveId: number;
    hrExecutiveId: number;
    logisticsExecutiveId: number;
    remarks: string;
    enquiryItems: IEnquiryItem[];
}


export interface IEnquiryItem {
    id: number;
    enquiryId: number;
    srNo: string;
    categoryItemId: number;
    categoryName: string;
    quantity: string;
    maxCVsToSend: number;
    ecnr: string;
    assessmentRequired: string;
    evaluationRequired: string;
    hrExecutiveId: number;
    assessingSupId: number;
    assessingHRMId: number;
    completeBy: string;
    reviewStatus: string;
    enquiryStatus: string;
    charges: string;
    remuneration: IRemuneration;
    jobDesc: IJobDesc;
}

export interface IRemuneration {
    id: number;
    enquiryItemId: number;
    contractPeriodInMonths: number;
    salaryCurrency: string;
    salaryMin: number;
    salaryMax: number;
    salaryNegotiable: string;
    housing: string;
    housingAllowance: number;
    food: string;
    foodAllowance: number;
    transport: string;
    transportAllowance: number;
    otherAllowance: number;
    leaveAvailableAfterHowmanyMonths: number;
    leaveEntitlementPerYear: number;
    updatedOn: string;
}

export interface IJobDesc {
    id: number;
    enquiryItemId: number;
    jobDescription: string;
    qualificationDesired: string;
    experienceDesiredMin: number;
    experienceDesiredMax: number;
    jobProfileDetails: string;
    jobProfileUrl: string;
}
