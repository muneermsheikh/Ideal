import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CandidateParams } from '../shared/models/CandidateParams';
import { ISource } from '../shared/models/candidateSource';
import { IClient, IClientOfficial } from '../shared/models/client';
import { ICandidate } from '../shared/models/ICand';
import { ICategoryWithProf } from '../shared/models/ICategoryWithProf';
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
  private candidateSource = new BehaviorSubject<ICandidate>(null);


  constructor(private http: HttpClient) { }

  addCandidate(values: any): any {
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

  deleteCandidate(id: number): any {
    return this.http.delete(this.baseUrl + 'HR/candidate?id=' + id).pipe(
      map((response: number) => {
        if (response !== 0) {
          console.log('candidate deleted');
          this.candidateSource.next(null);
        }
        }, error => {
          console.log(error);
        }
      )
    );
  }


  updateCandidate(values: ICandidate): any {
    console.log(values);
    return this.http.put(this.baseUrl + 'HR/candidate', values).pipe(
      map((cand: ICandidate) => {
        if (cand) {
          console.log('candidate ' + cand.fullName + ' updated'); }
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

  getCustomersData(): any {
    return this.http.get<IClient[]>(this.baseUrl + 'customers');
  }

  getCustomerOfficialsData(): any {
    return this.http.get<IClientOfficial[]>(this.baseUrl + 'customers/officials');
  }

  getCandCatsWithProf(): any {
    return this.http.get<ICategoryWithProf[]>(this.baseUrl + 'HR/CandCatWithProf');
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
          this.paginationCandidate = response.body;
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

// enquiries


}

