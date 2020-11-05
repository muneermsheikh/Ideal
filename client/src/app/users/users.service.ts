import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CandidateParams } from '../shared/models/CandidateParams';
import { ISource } from '../shared/models/candidateSource';
import { IClient } from '../shared/models/client';
import { ICandidate } from '../shared/models/ICand';
import { IPaginationCandidate, PaginationCandidate } from '../shared/models/paginationCand';
import { IProfession } from '../shared/models/profession';


@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private baseUrl = environment.apiUrl;
  candidates: ICandidate[] = [];
  paginationCandidate = new PaginationCandidate();
  candParams = new CandidateParams();


  constructor(private http: HttpClient) { }

  addCandidate(values: any): any {
    console.log(values);
    console.log(JSON.stringify(values));
    return this.http.post(this.baseUrl + 'HR/registercandidate', values).pipe(
      map((cand: ICandidate) => {
        if (cand) {
          console.log(cand);
        }
      }, error => {
        console.log(error);
      })
    );
  }


  /* delete returns integer - correct flg

  deleteCandidate(values: ICandidate) {
    return this.http.delete(this.baseUrl + 'HR/candidate', values).pipe(
      map((cand: number) => {
        if (cand !== 0) {
          console.log('candidate deleted'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }
*/

  updateCandidate(values: ICandidate): any {
    return this.http.put(this.baseUrl + 'HR/candidate', JSON.stringify(values)).pipe(
      map((prof: IProfession) => {
        if (prof) {
          console.log('profession ' + prof.name + ' updated'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }


  getCandidate(id: number): any {
    const candidate = this.candidates.find(p => p.id === id);
    console.log('users service');
    console.log(candidate);
    if (candidate) {
      return of(candidate);
    }
    return this.http.get<ICandidate>(this.baseUrl + 'HR/getcandidate/' + id);
  }

  getProfessions(): any {
    return this.http.get<IProfession[]>(this.baseUrl + 'Category/categories');
  }

  getCandidateSources(): any {
    return this.http.get<ISource[]>(this.baseUrl + 'HR/candidateSources');
  }

  getRecruitmentAgencies(): any {
    return this.http.get<IClient[]>(this.baseUrl + 'Customers/recruitmentAgencies');
  }

  getCandidates(useCache: boolean): any {

    // console.log('entered userService.getCandidates');

    if (useCache === false) {
      this.candidates = [];
    }

    if (this.candidates.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.candidates.length / this.candParams.pageSize);

      if (this.candParams.pageNumber <= pagesReceived) {
        this.paginationCandidate.data =
          this.candidates.slice((this.candParams.pageNumber - 1) * this.candParams.pageSize,
            this.candParams.pageNumber * this.candParams.pageSize);

        return of(this.paginationCandidate);
      }
    }

    let params = new HttpParams();

    if (this.candParams.professionId !== 0) {
      params = params.append('professionId', this.candParams.professionId.toString());
    }

    if (this.candParams.cityName !== '') {
      params = params.append('cityName', this.candParams.cityName);
    }

    if (this.candParams.search)
    {
      params = params.append('search', this.candParams.search);
    }

    params = params.append('sort', this.candParams.sort);

    params = params.append('pageIndex', this.candParams.pageNumber.toString());
    params = params.append('pageSize', this.candParams.pageSize.toString());
    console.log(params);

    return this.http.get<IPaginationCandidate>(this.baseUrl + 'HR/candidates', { observe: 'response', params })
      .pipe(
        map(response => {
          this.candidates = [...this.candidates, ...response.body.data];
          console.log(this.candidates);
          this.paginationCandidate = response.body;
          // console.log('getCandidates returned ' + response.body.count);
          return this.paginationCandidate;
        }, error => {
          console.log(error);
        })
      );
  }

  getCandParams(): CandidateParams {
    return this.candParams;
  }

  setCandParams(params: CandidateParams): void {
    this.candParams = params;
  }

  checkCandidateExists(ppnumber: string, aadharnumber: string): any {
    const exists =  this.http.get(this.baseUrl +
      'HR/candexists?appnumber=0 &ppnumber=' + ppnumber + '&aadharnumber=' + aadharnumber + '&email=""');
    return exists === null ? false : true;
  }

  checkPPNoExists(ppno: string): any {
    return this.http.get(this.baseUrl + 'hr/ppnoexists?ppnumber=' + ppno);
  }

  checkAadharNoExists(aadharno: string): any {
    return this.http.get(this.baseUrl + 'hr/aadharnoexists?aadharno=' + aadharno);
  }



}

