export interface IEmployee {

    id: number;
    firstName: string;
    secondName: string;
    familyName: string;
    gender: string;
    knownAs: string;
    fullName: string;
    dOB: string;
    pPNo: string;
    aadharNo: string;
    mobile: string;
    email: string;
    addresses: EmployeeAddress[];
  }

export interface EmployeeAddress {
      addressType: string;
      address1: string;
      address2: string;
      city: string;
      pin: string;
      state: string;
      district: string;
      country: string;
      valid: boolean;
  }
