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
  referredById: number;
  sourceId: number;
  eCNR: boolean;
  dOB: string;
  aadharNo: string;
  mobileNo: string;
  email: string;
  contactPreference: string;
  candidateAddresses: CandidateAddress[];
  candidateProfessions: Profession[];
}

export class Profession {
  id: number;
  name: string;
}

export class CandidateCategory {
  candId: number;
  catId: number;
}

export interface CandidateAddress {
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
