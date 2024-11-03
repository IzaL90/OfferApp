import { test, expect } from '@playwright/test';
import { SidebarPage } from '../pageObjects/SidebarPage';
import { TablePage } from '../pageObjects/TablePage';
import { SitePage } from '../pageObjects/SitePage';

test.beforeEach('Go to page', async ({ page }) => {
    await page.goto('http://localhost:5050');

})

test('Smoke tests', async ({ page }) => {
    await expect(page.locator("//table[@class='table table-striped']")).toBeVisible()
    await expect(page.locator("//h1[@data-name='offers-information']")).toHaveText("Hello, welcome to bid management!")
});

test('Sidebar test', async ({ page }) => {
    const sidebar = new SidebarPage(page)
    //await page.goto('http://localhost:5050');
    await sidebar.isVisible()
    await sidebar.clickHome()
    await expect(sidebar.clickedHome).toHaveClass(/active/)
    await sidebar.clickAperture()
});

test('Table visibility test', async ({ page }) => {
    const table = new TablePage(page)
    await table.isVisible()
    await table.expectColumns()
});

test('Add button test', async ({ page }) => {
    const site = new SitePage(page)
    await site.isVisible()
    await site.expectText()
});