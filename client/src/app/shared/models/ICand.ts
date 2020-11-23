export interface ICandidate {
  id: number;
  applicationNo: number;
  applicationDated: string;
  gender: string;
  firstName: string;
  secondName: string;
  familyName: string;
  knownAs: string;
  fullName: string;
  passportNo: string;
  referredById: number;
  sourceId: number;
  ecnr: string;
  dateOfBirth: string;
  aadharNo: string;
  mobileNo: string;
  email: string;
  contactPreference: string;
  address1: string;
  address2: string;
  city: string;
  pin: string;
  state: string;
  country: string;
  candidateCategories: ICandidateCategory[];  // IProfession[];
}

export interface IProfession {
  id: number;
  name: string;
  industryTypeId: number;
  skillLevelId: number;
  industryType: string;
  skillLevel: string;
}

export interface ICandidateCategory {
  candidateId: number;
  categoryId: number;
  name: string;
}

export interface ICandidateAddress {
    id: number;
    candidateId: number;
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
