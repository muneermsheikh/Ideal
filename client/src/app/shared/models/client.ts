export interface IClient{
    id: number;
    customerName: string;
    knownAs: string;
    cityName: string;
    introducedBy: number;
    email: string;
    mobile: string;
    phone: string;
    companyUrl: string;
    description: string;
    customerAddressId: number;
    addedOn: string;
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
        isValid: boolean;
        addedOn: string;
    }
