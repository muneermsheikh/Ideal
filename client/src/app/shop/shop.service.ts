import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IIndustryType } from '../shared/models/industryType';
import { IPagination, Pagination } from '../shared/models/pagination';
import { ISkillLevel } from '../shared/models/skillLevel';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { ICategory } from '../shared/models/category';


@Injectable({
  providedIn: 'root'
})

export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  categories: ICategory[] = [];
  indTypes: IIndustryType[] = [];
  skillLevels: ISkillLevel[] = [];
  pagination = new Pagination();
  shopParams = new ShopParams();

  constructor(private http: HttpClient) { }

  getCategories(shopParams: ShopParams): any {

    let params = new HttpParams();

    if (shopParams.industryTypeId !== 0) {
      params = params.append('industryTypeId', shopParams.industryTypeId.toString());
    }

    if (shopParams.skillLevelId !== 0) {
      params = params.append('skillLevelId', shopParams.skillLevelId.toString());
    }

    if (shopParams.search)
    {
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);

    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());


    return this.http.get<IPagination>(this.baseUrl + 'categories', { observe: 'response', params })
      .pipe(
        map(response => {
          this.categories = [...this.categories, ...response.body.data];
          this.pagination = response.body;
          return this.pagination;
        })
      );

  }

  getIndustryTypes(): any {
    return this.http.get<IIndustryType[]>(this.baseUrl + 'category/indTypes');
  }

  getSkillLevels(): any {
    return this.http.get<ISkillLevel[]>(this.baseUrl + 'category/skillLevels');
  }

  getShopParams(): any {
    return this.shopParams;
  }

  setShopParams(params: ShopParams): void {
    this.shopParams = params;
  }

}
