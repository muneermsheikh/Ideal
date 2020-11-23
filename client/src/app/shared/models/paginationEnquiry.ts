import { IEnquiry } from './enquiry';

export interface IPaginationEnquiry {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEnquiry[];
  }

export class PaginationEnquiry implements IPaginationEnquiry {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEnquiry[] = [];
}
