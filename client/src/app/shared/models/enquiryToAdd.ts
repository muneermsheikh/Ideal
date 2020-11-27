export interface IEnquiryToAdd {
    id: number;
    customerId: number;
    enquiryRef: string;
    enquiryDate: string;
    remarks: string;
    enquiryItems: IEnquiryItemToAdd[];
}


export interface IEnquiryItemToAdd {
    id: number;
    categoryItemId: number;
    quantity: number;
    maxCVsToSend: number;
    ecnr: string;
    assessmentRequired: string;
    evaluationRequired: string;
    charges: string;
    remuneration: IRemunerationToAdd;
    jobDesc: IJobDescToAdd;
}

export interface IRemunerationToAdd {
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

export interface IJobDescToAdd {
    id: number;
    jobDescription: string;
    qualificationDesired: string;
    experienceDesiredMin: number;
    experienceDesiredMax: number;
    jobProfileDetails: string;
    jobProfileUrl: string;
}
