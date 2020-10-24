export interface IUser {
  displayName: string;
  email: string;
  token: string;
  employeeId: number;
}

export interface IEmployee {
    displayName: string;
    gender: string;
    email: string;
    firstName: string;
    dob: Date;
    doj: Date;
    position: string;
    dept: string;
    token: string;
    employeeId: number;
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



