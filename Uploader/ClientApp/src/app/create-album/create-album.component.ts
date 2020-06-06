import { Component, OnInit, Inject } from '@angular/core';
import { CustomerService } from '../services/customer.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { IdResponse } from '../models/id-response.model';
import { BehaviorSubject } from 'rxjs';
import { FileUploadService } from '../services/file-upload.service';

@Component({
  selector: 'app-create-album',
  templateUrl: './create-album.component.html',
  styleUrls: ['./create-album.component.css']
})
export class CreateAlbumComponent implements OnInit {
  customerList: string[];
  form: FormGroup;
  uploadForm: FormGroup;
  baseUrl: string;
  progressGroup: number[] = [];
  albumId: string;

  imageIndex$ = new BehaviorSubject<number>(undefined);

  constructor(
    public fb: FormBuilder,
    private http: HttpClient,
    private customerService: CustomerService,
    public fileUploadService: FileUploadService,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.form = this.fb.group({
      albumName: [''],
      customerName: ['']
    });

    this.uploadForm = this.fb.group({
      albumId: [''],
      albumImages: [null]
    });

    this.baseUrl = baseUrl;

    this.imageIndex$.subscribe(
      imageIndex => {
        console.log('Image Index: ' + imageIndex);
        if (imageIndex !== undefined) {
          this.submitImage(imageIndex);
        }
      }
    )
  }

  ngOnInit() {
    this.customerService.GetAllCustomerNames().subscribe(
      (result: string[]) => this.customerList = result);
  }

  submitAlbum() {
    const formData = new FormData();
    formData.append("name", this.form.value.albumName);
    formData.append("customerName", this.form.value.customerName);

    this.http.post(this.baseUrl + 'album/create', formData).subscribe(
      (result: IdResponse) => {
        if (!result.errorsOccurred) {
          this.uploadForm.value.albumId = result.id;
          this.beginUpload();
        } else {
          // TODO: Handle error from server without a server error.
        }
      },
      err => {
        console.error(err);
      }
    )
  }

  submitImage(imageIndex: number) {
    if (imageIndex === undefined) {
      return;
    }
    this.fileUploadService.sendImage(
      this.uploadForm.value.albumId,
      this.uploadForm.value.albumImages[imageIndex]
    ).subscribe((event: HttpEvent<any>) => {
      switch (event.type) {
        case HttpEventType.Sent:
          console.log('Request has been made!');
          break;
        case HttpEventType.ResponseHeader:
          console.log('Response header has been received!');
          break;
        case HttpEventType.UploadProgress:
          this.progressGroup[imageIndex] = Math.round(event.loaded / event.total * 100);
          console.log(`Uploaded! ${this.progressGroup[imageIndex]}%`);
          break;
        case HttpEventType.Response:
          console.log('Image Successfully Sent!', event.body);
          const nextIndex = this.imageIndex$.value + 1;
          if (nextIndex < this.uploadForm.value.albumImages.length) {
            this.imageIndex$.next(nextIndex);
          }
      }
    });
  }

  uploadFile(event) {
    const files = (event.target as HTMLInputElement).files;
    console.log(files);
    this.uploadForm.patchValue({
      albumImages: files
    });
    this.uploadForm.get('albumImages').updateValueAndValidity();
    this.initializeProgressGroup(files.length);
    console.log(this.form);
  }

  initializeProgressGroup(groupLength: number) {
    for (let i=0; i<groupLength; i++) {
      this.progressGroup[i] = 0;
    }
  }

  beginUpload() {
    this.imageIndex$.next(0);
  }
}
