import { Injectable, Inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class FileUploadService {
    baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      this.baseUrl = baseUrl;
   }

  sendImage(id: string, albumImage: File): Observable<any> {
    var formData: any = new FormData();
    formData.append("ContainerId", id);
    formData.append("albumImage", albumImage);

    return this.http.post(this.baseUrl + 'upload', formData, {
      reportProgress: true,
      observe: 'events',
      responseType: 'text'
    }).pipe(
      catchError(this.errorMgmt)
    )
  }

  getReq(): Observable<any> {
      return this.http.get(this.baseUrl + 'upload');
  }

  errorMgmt(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }

}