import { IEmail } from './email';


export interface IPaginationEmail {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEmail[];
  }

export class PaginationEmail implements IPaginationEmail {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEmail[] = [];
}
