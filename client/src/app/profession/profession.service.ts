import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { IPagination, Pagination } from '../shared/models/pagination';
import { ProfParams } from '../shared/models/profParams';
import { map } from 'rxjs/operators';
import { IProfession } from '../shared/models/profession';
import { IPaginationProf, PaginationProf } from '../shared/models/paginationProf';
import { IIndustryType } from '../shared/models/industryType';
import { ISkillLevel } from '../shared/models/skillLevel';

@Injectable({
  providedIn: 'root'
})
export class ProfessionService {
  private baseUrl = environment.apiUrl;
  professions: IProfession[] = [];
  paginationProf = new PaginationProf();
  profParams = new ProfParams();


  constructor(private http: HttpClient) { }

  getProfession(id: number) {
    console.log('enered profservice.getProfession');
    return this.http.get<IProfession>(this.baseUrl + 'category/' + id);
  }

  getProfessions(profParams: ProfParams) {
    console.log('entered profService.getProfessions');

    let params = new HttpParams();

    if (profParams.industryTypeId !== 0) {
      params = params.append('industryTypeId', profParams.industryTypeId.toString());
    }

    if (profParams.skillLevelId !== 0) {
      params = params.append('skillLevelId', profParams.skillLevelId.toString());
    }

    if (profParams.search)
    {
      params = params.append('search', profParams.search);
    }

    params = params.append('sort', profParams.sort);

    params = params.append('pageIndex', profParams.pageNumber.toString());
    params = params.append('pageSize', profParams.pageSize.toString());
    console.log(params);

    return this.http.get<IPaginationProf>(this.baseUrl + 'categories', { observe: 'response', params })
      .pipe(
        map(response => {
          this.professions = [...this.professions, ...response.body.data];
          this.paginationProf = response.body;
          return this.paginationProf;
        })
      );
  }

  addProfession(values: any) {
    return this.http.post(this.baseUrl + 'category', values).pipe(
      map((prof: IProfession) => {
        if (prof) {
          console.log('profession ' + prof.name + ' added'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }

  deleteProfession(values: IProfession) {
    return this.http.put(this.baseUrl + 'category', values).pipe(
      map((prof: IProfession) => {
        if (prof) {
          console.log('profession ' + values.name + ' deleted'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }

  updateProfession(values: IProfession) {
    return this.http.put(this.baseUrl + 'category', values).pipe(
      map((prof: IProfession) => {
        if (prof) {
          console.log('profession ' + prof.name + ' updated'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }

  getProfParams(): ProfParams {
    return this.profParams;
  }

  setProfParams(params: ProfParams): void {
    this.profParams = params;
  }

  checkRecordExists(prof: string, indId: number, skillId: number) {
    const exists =  this.http.get(this.baseUrl +
      'category/exists?cat=' + prof + '&industryTypeId=' + indId + '&skillLevelId=' + skillId);
    return exists === null ? false : true;
  }


  getIndustryTypes(): any {
    return this.http.get<IIndustryType[]>(this.baseUrl + 'category/IndustryTypesWoPagination');
  }

  getSkillLevels(): any {
    return this.http.get<ISkillLevel[]>(this.baseUrl + 'category/SkillLevelsWoPagination');
  }
}
