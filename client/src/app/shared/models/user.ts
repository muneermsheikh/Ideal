import { IEmployeeAddress, ISkill } from './employee';

export interface IUser {
  displayName: string;
  email: string;
  token: string;
  employeeId: number;
}

export interface IEmployee {
    id: number;
    displayName: string;
    knownAs: string;
    gender: string;
    email: string;
    aadharNo: string;
    passportNo: string;
    mobile: string;
    firstName: string;
    secondName: string;
    familyName: string;
    dateOfBirth: Date;
    dateOfJoining: Date;
    designation: string;
    department: string;
    token: string;
    employeeId: number;
    skills: ISkill[];
    addresses: IEmployeeAddress[];
  }

export interface ICompany {
    companyName: string;
    displayName: string;
    cityName: string;
    gender: string;
    officialName: string;
    email: string;
    token: string;
    companyId: number;
  }

export interface IAssociate {
    companyName: string;
    displayName: string;
    cityName: string;
    gender: string;
    officialName: string;
    email: string;
    token: string;
    companyId: number;
  }



