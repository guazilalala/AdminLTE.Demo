var selectedRole = 0;
var selectedRootId = "";
var selectedRootName = "";
$(function () {
    $("#btnAdd").click(function () { add(); });
    $("#btnAddRoot").click(function () { addRoot(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    $("#btnSave").click(function () {
        var dictonaryFrom = $('#DictionaryForm');
        var bv = dictonaryFrom.data('bootstrapValidator');
        bv.validate();
        if (bv.isValid()) {
            save();
            $('#DictionaryForm').data('bootstrapValidator').resetForm(); //重置验证
        }
    });
    $("#checkAll").click(function () { checkAll(this) });
    $("#btnLoadRoot").click(function () {
        loadTables("00000000-0000-0000-0000-000000000000", 1, 10);
        $("#btnDelete").hide();
    });
    $("#btnCancel").click(function () { cancel(); });
    $("#formClose").click(function () { cancel(); });
    getTree();
    validatorIt()

});

//加载字典分类树
function getTree() {
    $.ajax({
        type: "Get",
        url: "/DataDictionary/GetDictionaryTreeData?_t=" + new Date().getTime(),    //获取数据的ajax请求地址
        dataType: "json",
        success: function (result) {
            $('#tree').treeview({
                data: result,                              // 数据源
                showCheckbox: false,                       //是否显示复选框
                highlightSelected: true,                   //是否高亮选中
                //nodeIcon: 'glyphicon glyphicon-user',    //节点上的图标
                color: '#428bca',                          //颜色
                nodeIcon: 'glyphicon glyphicon-bookmark',  //图标
                emptyIcon: '',                             //没有子节点的节点图标
                multiSelect: false,                        //多选
                onNodeChecked: function (event, data) {

                },
                onNodeSelected: function (event, data) {
                    selectedRootId = data.id;
                    selectedRootName = data.text;
                    loadTables(data.id, 1, 10);
                    $("#btnDelete").show();
                }
            });
        },
        error: function () {
            alert("树形结构加载失败！")
        }
    });
};
//加载列表数据
function loadTables(id, startPage, pageSize) {
    $("#tableBody").html("");
    $("#checkAll").prop("checked", false);
    $.ajax({
        type: "GET",
        url: "/DataDictionary/GetPageListByGuid?id=" + id + "&startPage=" + startPage + "&pageSize=" + pageSize + "&_t=" + new Date().getTime(),
        success: function (data) {
            $.each(data.rows, function (i, item) {
                var tr = "<tr>";
                tr += "<td align='center'><input type='checkbox' class='checkboxs' value='" + item.id + "'/></td>";
                tr += "<td>" + (item.parentId == "00000000-0000-0000-0000-000000000000" ? "无" : selectedRootName) + "</td>";
                tr += "<td>" + item.name + "</td>";
                tr += "<td>" + (item.code == null ? "" : item.code) + "</td>";
                tr += "<td>" + (item.value == null ? "" : item.value) + "</td>";
                tr += "<td>" + (item.serialNumber == null ? "" : item.serialNumber) + "</td>";
                tr += "<td>" + (item.isEnabled == true ? "可用" : "禁用") + "</td>";
                tr += "<td><button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" + item.id + "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" + item.id + "\")'><i class='fa fa-trash-o'></i> 删除 </button> </td>"
                tr += "</tr>";
                $("#tableBody").append(tr);
            })
            var elment = $("#grid_paging_part"); //分页插件的容器id
            if (data.rowCount > 0) {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.rowsCount, //总数
                    totalPages: data.pageCount, //总页数
                    onPageChanged: function (event, oldPage, newPage) { //页面切换事件

                        if (data.isRootList == true) {
                            loadTables("00000000-0000-0000-0000-000000000000", newPage, pageSize)
                        }
                        else {
                            loadTables(selectedRootId, newPage, pageSize)
                        }
                        
                    }
                }
                elment.bootstrapPaginator(options); //分页插件初始化
            }
            else {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.rowsCount, //总数
                    totalPages: data.pageCount, //总页数
                    onPageChanged: function (event, oldPage, newPage) { //页面切换事件
                    }
                }
                elment.bootstrapPaginator(options); //分页插件初始化
            }
        }
    })
}
//全选
function checkAll(obj) {
    $(".checkboxs").each(function () {
        if (obj.checked == true) {
            $(this).prop("checked", true)

        }
        if (obj.checked == false) {
            $(this).prop("checked", false)
        }
    });
};
//新增
function add() {
    if (selectedRootId == "") {
        layer.alert("请选择字典分类！");
        return;
    }
    $("#Id").val("00000000-0000-0000-0000-000000000000");
    $("#Code").val("");
    $("#Name").val("");
    $("#Remarks").val("");
    $("#Title").text("新增下级字典");
    $("#Parent").val(selectedRootName);
    $("#Value").val("");
    $("#SerialNumber").val(0);
    $("#Enabled").prop('checked', true);
    $("#confirmAndClose").prop('checked', true);
    //弹出新增窗体
    $("#addDictionaryModal").modal("show");
};

function addRoot() {
    $("#Id").val("00000000-0000-0000-0000-000000000000");
    $("#ParentId").val("00000000-0000-0000-0000-000000000000");
    $("#Code").val("");
    $("#Name").val("");
    $("#Remarks").val("");
    $("#Title").text("新增根字典");
    $("#Parent").val("");
    $("#Parent").attr("disabled", "disabled");
    $("#Value").val("");
    $("#SerialNumber").val(0);
    $("#Enabled").prop('checked', true);
    $("#confirmAndClose").prop('checked', true);
    //弹出新增窗体
    $("#addDictionaryModal").modal("show");
};
//编辑
function edit(id) {
    $.ajax({
        type: "Get",
        url: "/DataDictionary/Get?id=" + id + "&_t=" + new Date().getTime(),
        success: function (data) {
            $("#Id").val(data.id);
            $("#Name").val(data.name);
            $("#Code").val(data.code);
            $("#Value").val(data.value);
            $("#SerialNumber").val(data.serialNumber);
            $("#Parent").val(selectedRootName);

            if (data.isEnabled == true) {
                $("#Enabled").prop('checked', true);
            }
            else {
                $("#Disabled").prop('checked', true);
            }
            $("#Remarks").val(data.remarks);
            $("#Title").text("编辑角色")
            $("#addDictionaryModal").modal("show");
        }
    })
};
//保存
function save() {
    var isEnabled = document.getElementById('Enabled').checked == true ? true : false;

    var parentId = $("#Parent").val() != "" ? selectedRootId : "00000000-0000-0000-0000-000000000000";

    var postData = {
        "dto": {
            "Id": $("#Id").val(),
            "Name": $("#Name").val(),
            "Code": $("#Code").val(),
            "Remarks": $("#Remarks").val(),
            "ParentId": parentId,
            "SerialNumber": $("#SerialNumber").val(),
            "Value": $("#Value").val(),
            "IsEnabled": isEnabled,
        }
    };
    $.ajax({
        type: "Post",
        url: "/DataDictionary/Edit",
        data: postData,
        success: function (data) {
            if (data.result == "Success") {

                if (document.getElementById('confirmAndClose').checked) {
                    $("#addDictionaryModal").modal("hide");
                }
                else {
                    $("#Id").val("00000000-0000-0000-0000-000000000000");
                    $("#Code").val("");
                    $("#Name").val("");
                    $("#Remarks").val("");
                    if ($("#Title").text() == "新增根字典") {
                        $("#Parent").val("");
                    }
                    else {
                        $("#Parent").val(selectedRootName);
                    }
                    $("#Value").val("");
                    $("#SerialNumber").val(0);
                    $("#Enabled").prop('checked', true);

                    $("#Name").focus();
                }


                if ($("#Parent").val() == "") {
                    getTree();
                }
                loadTables(selectedRootId, 1, 10);
            } else {
                layer.tips(data.message, "#btnSave", {
                    tips: [1, '#c9302c']
                });
            };
        }
    });
};
//批量删除
function deleteMulti() {
    var ids = "";
    $(".checkboxs").each(function () {
        if ($(this).prop("checked") == true) {
            ids += $(this).val() + ","
        }
    });
    ids = ids.substring(0, ids.length - 1);
    if (ids.length == 0) {
        layer.alert("请选择要删除的记录。");
        return;
    };
    //询问框
    layer.confirm("您确认删除选定的记录吗？", {
        btn: ["确定", "取消"]
    }, function () {
        var sendData = { "ids": ids };
        $.ajax({
            type: "Post",
            url: "/DataDictionary/DeleteMuti",
            data: sendData,
            success: function (data) {
                if (data.result == "Success") {
                    loadTables(selectedRootId, 1, 10)
                    layer.closeAll();
                }
                else {
                    layer.alert("删除失败！");
                }
            }
        });
    });
};
//删除单条数据
function deleteSingle(id) {
    layer.confirm("您确认删除选定的记录吗？", {
        btn: ["确定", "取消"]
    }, function () {
        $.ajax({
            type: "POST",
            url: "/DataDictionary/Delete",
            data: { "id": id },
            success: function (data) {
                if (data.result == "Success") {
                    if (data.parentId ="00000000-0000-0000-0000-000000000000") {
                        loadTables("00000000-0000-0000-0000-000000000000", 1, 10);
                    }
                    loadTables(selectedRootId, 1, 10)
                    getTree();
                    layer.closeAll();
                }
                else {
                    layer.alert(data.message);
                }
            }
        })
    });
};

//表单验证
function validatorIt() {
    $('#DictionaryForm').bootstrapValidator({
        message: "This value is not valid",
        feedbackIcons: {
            valid: "glyphicon glyphicon-ok",
            invalid: "glyphicon glyphicon-remove",
            validating: "glyphicon glyphicon-refresh"
        },
        fields: {
            name: {
                validators: {
                    notEmpty: {
                        message: "名称不能为空"
                    }
                }
            },
            code: {
                validators: {
                    notEmpty: {
                        message: "编码不能为空"
                    }
                }
            },
            value: {
                validators: {
                    notEmpty: {
                        message: "值不能为空"
                    }
                }
            }
        }
    });

};

//取消
function cancel() {
    $('#DictionaryForm').data('bootstrapValidator').resetForm(); //重置验证
    $("#addDictionaryModal").modal("hide");
};

