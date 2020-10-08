import { IProfession } from './profession';

export interface IPaginationProf {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IProfession[];
  }

export class PaginationProf implements IPaginationProf {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IProfession[] = [];
}
