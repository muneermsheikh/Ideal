import { ICategory } from './category';

export interface IPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICategory[];
  }

export class Pagination implements IPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: ICategory[] = [];
}