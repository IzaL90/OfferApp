import test, { expect, Page } from "@playwright/test";
import { Locator } from "playwright";
import { FormComponent } from "./FormComponent";

export class AddComponent {
  public readonly root: Locator;
  public readonly submit: Locator
  public readonly cancel: Locator
  public readonly close: Locator
  public readonly form: FormComponent

  constructor(root: Locator){
    this.root = root;
    this.submit = this.root.locator("//button[@data-name='bid-submit-button']")
    this.cancel = this.root.locator("//button[@data-name='bid-close-button']")
    this.close = this.root.locator("//button[@data-name='modal-close-button']")
    this.form = new FormComponent(this.root.locator("//form"))
  }

  public async isVisible(): Promise<boolean> {
    return this.root.isVisible();
  }
  
  public async expectModalVisible(): Promise<void> {
    expect(this.form.expectFormVisible)
    await expect(this.submit).toBeVisible()
    await expect(this.cancel).toBeVisible()
    await expect.soft(this.close).toBeVisible()
    
  }

  public async fillForm(): Promise<void> {
    test.slow()
    await this.form.name.fill('TestBidddddddd')
    await this.form.price.fill('7657')
    await this.form.description.fill('a description')
}

  public async clickSubmit():Promise<void>{
    await this.submit.click()
}

}