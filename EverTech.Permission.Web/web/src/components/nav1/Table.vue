<template>
	<section>
		<!--工具条-->
		<el-col :span="24" class="toolbar">
			<el-form :inline="true" :model="formInline" class="demo-form-inline">
				<el-form-item>
					<el-input v-model="formInline.user" placeholder="姓名"></el-input>
				</el-form-item>
				<el-form-item>
					<el-button>查询</el-button>
				</el-form-item>
				<el-form-item>
					<el-button @click="handleAdd">新增</el-button>
				</el-form-item>
			</el-form>
		</el-col>

		<!--列表-->
		<template>
			<el-table :data="tableData" highlight-current-row v-loading="listLoading" style="width: 100%;">
				<el-table-column type="index" width="50"></el-table-column>
				<el-table-column prop="Account" label="账号" width="100"></el-table-column>
				<el-table-column prop="Name" label="姓名" width="100"></el-table-column>
				<el-table-column prop="Gender" label="性别" width="100" :formatter="formatSex"></el-table-column>
				<el-table-column prop="Phone" label="手机" width="200"></el-table-column>
				<el-table-column prop="EMail" label="EMail" width="200"></el-table-column>
				<el-table-column prop="AddTime" label="创建时间"></el-table-column>
				<el-table-column prop="AddUser" label="添加人"></el-table-column>
				<el-table-column prop="Enable" label="启用" :formatter="formatEnable"></el-table-column>
				<el-table-column inline-template :context="_self" label="操作" width="100">
					<span>
						<el-button type="text" size="small" @click="handleEdit(row)">编辑</el-button>
						<el-button type="text" size="small" @click="handleDel(row)">删除</el-button>
				    </span>
				</el-table-column>
			</el-table>
		</template>

		<!--分页-->
		<el-col :span="24" class="toolbar" style="padding-bottom:10px;">
			<el-pagination :current-page="1" :page-sizes="[100, 200, 300, 400]" :page-size="100" layout="total, sizes, prev, pager, next, jumper"
				:total="400" style="float:right">
				</el-pagination>
		</el-col>

		<!--编辑界面-->
		<el-dialog :title="editFormTtile" v-model="editFormVisible" :close-on-click-modal="false">
			<el-form :model="editForm" label-width="80px" :rules="editFormRules" ref="editForm">
				<el-form-item label="账号" prop="Account">
					<el-input v-model="editForm.Account" auto-complete="off"></el-input>
				</el-form-item>
				<el-form-item label="性别" prop="Gender">
					<el-select v-model="editForm.Gender" placeholder="请选择性别">
						<el-option label="男" :value="1"></el-option>
						<el-option label="女" :value="0"></el-option>
					</el-select>
				</el-form-item>
				<el-form-item label="是否启用" prop="Enable">
					<el-radio-group v-model="editForm.Enable">
						<el-radio class="radio" :label="1">启用</el-radio>
						<el-radio class="radio" :label="0">禁用</el-radio>
					</el-radio-group>
				</el-form-item>
				<el-form-item label="姓名" prop="Name">
					<el-input v-model="editForm.Name" auto-complete="off"></el-input>
				</el-form-item>
				<el-form-item label="密码" prop="Password">
					<el-input v-model="editForm.Password" type="Password" auto-complete="off"></el-input>
				</el-form-item>
				<el-form-item label="手机号" prop="Phone">
					<el-input v-model="editForm.Phone"></el-input>
				</el-form-item>
				<el-form-item label="邮箱地址" prop="EMail">
					<el-input v-model="editForm.EMail"></el-input>
				</el-form-item>

			</el-form>
			<div slot="footer" class="dialog-footer">
				<el-button @click.native="editFormVisible = false">取 消</el-button>
				<el-button type="primary" @click.native="editSubmit" :loading="editLoading">{{btnEditText}}</el-button>
			</div>
		</el-dialog>
	</section>
</template>

<script>
	import util from '../../common/util'
	import NProgress from 'nprogress'

	export default {
		data() {
			return {
				formInline: {
					user: ''
				},
				pickerOptions0: {
					disabledDate(time) {
						return time.getTime() < Date.now() - 8.64e7;
					}
				},
				value1: '',
				editFormVisible: false,//编辑界面显是否显示
				editFormTtile: '编辑',//编辑界面标题
				//编辑界面数据
				editForm: {
					id: 0,
					name: '',
					sex: -1,
					age: 0,
					birth: '',
					addr: ''
				},
				editLoading: false,
				btnEditText: '提 交',
				editFormRules: {
					name: [
						{ required: true, message: '请输入姓名', trigger: 'blur' }
					]
				},
				listLoading: false,
				tableData: []
			}
		},
		mounted() {
			this.$http.get("http://localhost:2000/User/findpage/1").then(data => {
				console.log(JSON.stringify(data.body.Data))
				this.tableData = data.body.Data;
			}, response => {
				console.info(response);
			})
		},
		methods: {
			//性别显示转换
			formatSex: function (row, column) {
				return row.Gender == true ? '男' : row.Gender == false ? '女' : '未知';
			},
			//是否可用
			formatEnable: function (row, column) {
				return row.Enable == true ? '启用' : row.Enable == false ? '禁用' : '未知';
			},
			//删除记录
			handleDel: function (row) {
				//console.log(row);
				var _this = this;
				this.$confirm('确认删除该记录吗?', '提示', {
					//type: 'warning'
				}).then(() => {
					_this.listLoading = true;
					NProgress.start();
					setTimeout(function () {
						for (var i = 0; i < _this.tableData.length; i++) {
							if (_this.tableData[i].id == row.id) {
								_this.tableData.splice(i, 1);

								_this.listLoading = false;
								NProgress.done();
								_this.$notify({
									title: '成功',
									message: '删除成功',
									type: 'success'
								});

								break;
							}
						}
					}, 1000);
				}).catch(() => {

				});
			},
			//显示编辑界面
			handleEdit: function (row) {
				this.editFormVisible = true;
				this.editFormTtile = '编辑';
				this.editForm.id = row.id;
				this.editForm.name = row.name;
				this.editForm.sex = row.sex;
				this.editForm.age = row.age;
				this.editForm.birth = row.birth;
				this.editForm.addr = row.addr;
			},
			//编辑 or 新增
			editSubmit: function () {
				var _this = this;

				_this.$refs.editForm.validate((valid) => {
					if (valid) {

						_this.$confirm('确认提交吗？', '提示', {}).then(() => {
							_this.editLoading = true;
							NProgress.start();
							_this.btnEditText = '提交中';
							setTimeout(function () {
								_this.editLoading = false;
								NProgress.done();
								_this.btnEditText = '提 交';
								_this.$notify({
									title: '成功',
									message: '提交成功',
									type: 'success'
								});
								_this.editFormVisible = false;

								if (_this.editForm.id == 0) {
									//新增
									_this.$http.post("http://localhost:2000/api/User/add", _this.editForm).then(data => {
										console.log(JSON.stringify(data.body.Data))
										//this.tableData = data.body.Data;
									}, response => {
										console.info(response);
									})

								} else {
									//编辑
									for (var i = 0; i < _this.tableData.length; i++) {
										if (_this.tableData[i].id == _this.editForm.id) {
											_this.tableData[i].name = _this.editForm.name;
											_this.tableData[i].sex = _this.editForm.sex;
											_this.tableData[i].age = _this.editForm.age;
											_this.tableData[i].birth = _this.editForm.birth == '' ? '' : util.formatDate.format(new Date(_this.editForm.birth), 'yyyy-MM-dd');
											_this.tableData[i].addr = _this.editForm.addr;
											break;
										}
									}
								}
							}, 1000);

						});

					}
				});

			},
			//显示新增界面
			handleAdd: function () {
				var _this = this;

				this.editFormVisible = true;
				this.editFormTtile = '新增';

				this.editForm.id = 0;
				this.editForm.name = '';
				this.editForm.sex = 1;
				this.editForm.age = 0;
				this.editForm.birth = '';
				this.editForm.addr = '';
			}
		}
	}

</script>

<style scoped>
	.toolbar .el-form-item {
		margin-bottom: 10px;
	}
	
	.toolbar {
		background: #fff;
		padding: 10px 10px 0px 10px;
	}
	/*.el-dialog .el-input,
	.el-dialog .el-select {
		width: 250px;
	}*/
</style>