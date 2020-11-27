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
    contractPeriodInMonths: string;
    salaryCurrency: string;
    salaryMin: string;
    salaryMax: string;
    salaryNegotiable: string;
    housing: string;
    housingAllowance: string;
    food: string;
    foodAllowance: string;
    transport: string;
    transportAllowance: number;
    otherAllowance: string;
    leaveAvailableAfterHowmanyMonths: string;
    leaveEntitlementPerYear: string;
    updatedOn: string;
}

export interface IJobDesc {
    id: number;
    jobDescription: string;
    qualificationDesired: string;
    experienceDesiredMin: number;
    experienceDesiredMax: number;
    jobProfileDetails: string;
    jobProfileUrl: string;
}
