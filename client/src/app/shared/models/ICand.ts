export interface ICandidate {
  id: number;
  applicationNo: number;
  applicationDated: string;
  addedOn: string;
  gender: string;
  firstName: string;
  secondName: string;
  familyName: string;
  knownAs: string;
  fullName: string;
  categoryName: string;
  professionId: number;
  ppNo: string;
  eCNR: boolean;
  dOB: string;
  aadharNo: string;
  mobileNo: string;
  email: string;
  candidateAddress: CandAddress;
  candidateCategories: CandProfession[];
}

export class CandProfession {
  id: number;
  name: string;
}

export interface CandAddress {
    candidateId: number;
    addressType: string;
    address1: string;
    address2: string;
    city: string;
    pIN: string;
    state: string;
    district: string;
    country: string;
    valid: boolean;
}
