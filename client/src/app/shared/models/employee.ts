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
    department: string;
    designation: string;
    addresses: IEmployeeAddress[];
    skills: ISkill[];
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

export interface ISkill {
      skillName: string;
      expInYears: string;
      proficiency: string;
  }
