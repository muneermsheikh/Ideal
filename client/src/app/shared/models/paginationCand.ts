import { ICandidate } from './ICand';


export interface IPaginationCandidate {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICandidate[];
  }

export class PaginationCandidate implements IPaginationCandidate {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICandidate[] = [];
}
