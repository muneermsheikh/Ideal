export interface IEmployee {

    id: number;
    firstName: string;
    secondName: string;
    familyName: string;
    gender: string;
    knownAs: string;
    fullName: string;
    dateOfBirth: string;
    dateOfJoining: string;
    passportNo: string;
    aadharNo: string;
    mobile: string;
    email: string;
/*    address1: string;
    address2: string;
    city: string;
    pin: string;
    state: string;
*/
    addresses: IEmployeeAddress[];
    employeeSkills: ISkills[];
  }

export interface IEmployeeAddress {
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

export interface ISkills {
      skillName: string;
      expInYrs: number;
      proficiency: string;
  }
