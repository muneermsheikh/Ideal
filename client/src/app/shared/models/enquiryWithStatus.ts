export interface IEnquiryWithStatus
{
    id: number;
    customerId : number;
    customerName: string;
    city: string;
    country: string;
    enquiryNo: number;
    enquiryDate: string;
    enquiryRef: string;
    basketId: string;
    noOfCategories: number;
    sumOfQuantities: number;
    completeBy: string;
    projectManager: string;
    assignedToHRExecutives: string;
    reviewStatus: string;
    enquiryStatus: string;
}