import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FileUploadService } from '../services/file-upload.service';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrls: ['./uploader.component.css']
})
export class UploaderComponent {
  form: FormGroup;
  progressGroup: number[] = [];
  imageIndex$ = new BehaviorSubject<number>(undefined);

  constructor(
    public fb: FormBuilder,
    public fileUploadService: FileUploadService
  ) {
    this.form = this.fb.group({
      name: [''],
      albumImages: [null]
    });

    this.imageIndex$.subscribe(
      imageIndex => {
        console.log('Image Index: ' + imageIndex);
        if (imageIndex !== undefined) {
          this.submitImage(imageIndex);
        }
      }
    )
  }

  initializeProgressGroup(groupLength: number) {
    for (let i=0; i<groupLength; i++) {
      this.progressGroup[i] = 0;
    }
  }

  uploadFile(event) {
    const files = (event.target as HTMLInputElement).files;
    console.log(files);
    this.form.patchValue({
      albumImages: files
    });
    this.form.get('albumImages').updateValueAndValidity();
    this.initializeProgressGroup(files.length);
    console.log(this.form);
  }

  beginUpload() {
    this.imageIndex$.next(0);
  }

  submitImage(imageIndex: number) {
    if (imageIndex === undefined) {
      return;
    }
    this.fileUploadService.sendImage(
      this.form.value.name,
      this.form.value.albumImages[imageIndex]
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
          if (nextIndex < this.form.value.albumImages.length) {
            this.imageIndex$.next(nextIndex);
          }
      }
    });
  }

}
