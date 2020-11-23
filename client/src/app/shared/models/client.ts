
export interface IClient{
    id: number;
    customerType: string;
    customerName: string;
    knownAs: string;
    address1: string;
    address2: string;
    city: string;
    pin: string;
    state: string;
    country: string;
    introducedBy: string;
    email: string;
    mobile: string;
    phone: string;
    companyUrl: string;
    description: string;
    customerStatus: string;
    addedOn: string;
    customerOfficials: IClientOfficial[];
    customerIndustryTypes: ICustomerIndustryType[];
  }

export interface IClientOfficial {
        id: number;
        customerId: number;
        name: string;
        designation: string;
        gender: string;
        phone: string;
        mobile: string;
        mobile2: string;
        email: string;
        personalEmail: string;
        personalMobile: string;
        // isValid: string;
        addedOn: [string, null];
    }

export interface ICustomerIndustryType {
    id: number;
    customerId: number;
    industryTypeId: number;
    name: string;
}
