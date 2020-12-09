import { IReview } from './review';

export interface IPaginationReview {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IReview[];
  }

export class PaginationReview implements IPaginationReview {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IReview[] = [];
}
