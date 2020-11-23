
import { IEnquiryWithStatus } from './enquiryWithStatus';

export interface IPaginationEnquiryWithStatus {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEnquiryWithStatus[];
  }

export class PaginationEnquiryWithStatus implements IPaginationEnquiryWithStatus {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEnquiryWithStatus[] = [];
}
