import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';


import { IPagination, Pagination } from '../shared/models/pagination';
import { ProfParams } from '../shared/models/profParams';
import { map } from 'rxjs/operators';
import { IProfession } from '../shared/models/profession';
import { IPaginationProf, PaginationProf } from '../shared/models/paginationProf';
import { IIndustryType } from '../shared/models/industryType';
import { ISkillLevel } from '../shared/models/skillLevel';
import { ICategory } from '../shared/models/category';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfessionService {
  private baseUrl = environment.apiUrl;
  professions: IProfession[] = [];
  indTypes: IIndustryType[] = [];
  skillLevels: ISkillLevel[] = [];
  paginationProf = new PaginationProf();
  profParams = new ProfParams();


  constructor(private http: HttpClient) { }

  getProfession(id: number): any {

    const profession = this.professions.find(p => p.id === id);

    if (profession) {
      return of(profession);
    }

    return this.http.get<IProfession>(this.baseUrl + 'category/' + id);
  }

  getProfessions(useCache: boolean): any {
    if (useCache === false) {
      this.professions = [];
    }

    if (this.professions.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.professions.length / this.profParams.pageSize);

      if (this.profParams.pageNumber <= pagesReceived) {
        this.paginationProf.data =
          this.professions.slice((this.profParams.pageNumber - 1) * this.profParams.pageSize,
            this.profParams.pageNumber * this.profParams.pageSize);

        return of(this.paginationProf);
      }
    }
    console.log('entered profService.getProfessions');

    let params = new HttpParams();

    if (this.profParams.industryTypeId !== 0) {
      params = params.append('industryTypeId', this.profParams.industryTypeId.toString());
    }

    if (this.profParams.skillLevelId !== 0) {
      params = params.append('skillLevelId', this.profParams.skillLevelId.toString());
    }

    if (this.profParams.search)
    {
      params = params.append('search', this.profParams.search);
    }

    params = params.append('sort', this.profParams.sort);
    params = params.append('pageIndex', this.profParams.pageNumber.toString());
    params = params.append('pageSize', this.profParams.pageSize.toString());
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

  addProfession(values: any): any {
    return this.http.post(this.baseUrl + 'category', values
       , {headers: {'Content-Type': 'application/json'}}
      ).pipe(
      map((prof: IProfession) => {
        if (prof) {
          console.log('profession ' + prof.name + ' added'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }

  deleteProfession(values: IProfession): any {
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

  updateProfession(values: IProfession): any {
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

  checkRecordExists(prof: string, indId: number, skillId: number): any {
    const exists =  this.http.get(this.baseUrl +
      'category/exists?cat=' + prof + '&industryTypeId=' + indId + '&skillLevelId=' + skillId);
    return exists === null ? false : true;
  }


  getIndustryTypes(): any {
    if (this.indTypes.length > 0) {
      return of(this.indTypes);
    }
    return this.http.get<IIndustryType[]>(this.baseUrl + 'category/IndustryTypesWOPagination').pipe(
      map(response => {
        this.indTypes = response;
        return response;
      })
    );

    // return this.http.get<IIndustryType[]>(this.baseUrl + 'category/IndustryTypesWoPagination');
  }


   getSkillLevels(): any {
    if (this.skillLevels.length > 0) {
      return of(this.skillLevels);
    }
    return this.http.get<ISkillLevel[]>(this.baseUrl + 'category/SkillLevelsWOPagination').pipe(
      map(response => {
        this.skillLevels = response;
        return response;
      })
    );

    // return this.http.get<ISkillLevel[]>(this.baseUrl + 'category/SkillLevelsWoPagination');
  }
}
