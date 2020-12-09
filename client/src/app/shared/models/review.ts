export interface IReview {
    id: number;
    enquiryId: number;
    enquiryNo: number;
    enquiryDate?: string;
    customerId: number;
    customerName: string;
    reviewStatus: string;
    reviewedBy: string;
    reviewedOn: string;
    contractReviewItems: IReviewItem[];
}
export interface IReviewItem {
    id: number;
    enquiryId: number;
    enquiryItemId: number;
    categoryItemId: number;
    categoryName: string;
    quantity: number;
    technicallyFeasible: boolean;
    commerciallyFeasible: boolean;
    logisticallyFeasible: boolean;
    visaAvailable: boolean;
    documentationWillBeAvailable: boolean;
    historicalStatusAvailable: boolean;
    salaryOfferedFeasible: boolean;
    serviceChargesInINR: string;
    feeFromClientCurrency: string;
    feeFromClient: number;
    status: string;
    reviewedOn: string;
    reviewedBy: number;
}

