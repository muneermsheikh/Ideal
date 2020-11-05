import { IEmployee } from './employee';


export interface IPaginationEmployee {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEmployee[];
  }

export class PaginationEmployee implements IPaginationEmployee {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEmployee[] = [];
}
