import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, ScrollView, TextInput, Image} from 'react-native';
import { Dropdown } from 'react-native-element-dropdown';
import { showMessage } from "react-native-flash-message";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';
import * as ImagePicker from "expo-image-picker";

import url from '../../../../services/url';
import api from '../../../../services/api';
import fonts from "../../../styles/fonts";

export default function CadastroEquipes({ route, navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const usuario = route.params?.usuario;

    const [image, setImage] = useState(null);
    const [nome_equipe, setNome_equipe] = useState('');
    const [categoriaEquipe, setCategoriaEquipe] = useState('');
    const [categoriaSelecionada, setCategoriaSelecionada] = useState(null);

    const campos = {
        nome_equipe,
        categoriaEquipe
    };

    const [dados, setDados] = useState([]); 
    const [sucess, setSucess] = useState(false);

//Buscar Categorias
    async function listarDados() {
        try {
            const res = await api.get(`dev4tec/categoria.php`, {
            params: {id_empresa: usuario.id_empresa }
            });

            if (res.data.success) {
            setDados(res.data.result || []);
            } else {
            console.log("Erro na API:", res.data.message);
            setDados([]);
            }
        }
        catch (error) {
            console.log("Erro ao listar categorias", error);
        }
        }


    useEffect(() => {
        listarDados();
    }, [usuario?.id_empresa]);


//Escolha de Imagem
    async function pickImageFromGallery() {
        let result = await ImagePicker.launchImageLibraryAsync({
            mediaTypes: ImagePicker.MediaTypeOptions.Images,
            allowsEditing: true,
            aspect: [4, 3],
            quality: 1,
        });

        if (!result.canceled) {
            console.log(result); // Verificar o retorno completo
            setImage(result.assets[0].uri); // Acesse o URI corretamente
        }
        }

    async function takePhoto() {
        let result = await ImagePicker.launchCameraAsync({
            allowsEditing: true,
            aspect: [4, 3],
            quality: 1,
        });

        if (!result.canceled) {
            console.log(result); // Verificar o retorno completo
            setImage(result.assets[0].uri); // Acesse o URI corretamente
        }
        }


//Upload da Imagem   
    async function uploadImage() {
        if (!image) {
            showMessage({
            message: 'Nenhuma imagem selecionada.',
            description: 'Por favor, selecione ou tire uma foto primeiro.',
            floating: true,
            statusBarHeight: 70,
            type: "danger",
            duration: 2000,             
            });
            return false;
        }

        let filename = image.split('/').pop();
        let match = /\.(\w+)$/.exec(filename);
        let type = match ? `image/${match[1]}` : `image`;

        let formData = new FormData();
        formData.append('photo', { uri: image, name: filename, type });

        try {
            const response = await fetch(`${url}/dev4tec/upload_equipe2.php`, {
                method: 'POST',
                body: formData,
            });
            
            const text = await response.text();
            let resJson;
            
            try {
                resJson = JSON.parse(text);
            } catch (e) {
                console.error("Erro ao converter JSON:", e);
            }

            if (response.ok && resJson.success) {        
                showMessage({
                    message: 'Sucesso.',
                    description: 'Imagem enviada com sucesso!',
                    floating: true,
                    statusBarHeight: 70,
                    type: "success",
                    duration: 2000,             
            });
            return resJson.url ?? resJson.file ?? resJson;
            
            } else {
                showMessage({
                message: 'Erro.',
                description: resJson.message || "Falha ao enviar imagem.",
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 2000,             
                });
                return false;
            }
        } catch (error) {
            console.error(error);
            showMessage({
                message: 'Erro.',
                description: "Ocorreu um erro ao tentar enviar a imagem.",
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 2000,             
            });
            return false;
        }
    }

//Cadastro da Equipe
    async function cadastrar() {   
        const foto_equipe = await uploadImage();
        if (!foto_equipe) return;

        const camposVazios = Object.entries(campos).filter(([_, valor]) => !valor.trim());    
        if (camposVazios.length > 0) {
            showMessage({
                message: "Erro Preencha todos os campos obrigatórios!",
                description: "Preencha todas as informações",
                floating: true,
                statusBarHeight: 70,
                type: "danger",
                duration: 2000,             
            });    
            return;
        }
        try {
            const res = await api.post('dev4tec/cadastroequipe.php', {
                nome_equipe,
                id_categoria: categoriaSelecionada,
                id_empresa: usuario.id_empresa,
                foto_equipe,
                AdminId: usuario.AdminId
            });

            if (res.data.sucesso === false) {

            showMessage({
                message: "Erro ao Cadastrar",
                description: res.data.mensagem,
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 3000,                    
            });  
            limparCampos();            
            return;
            }

            setSucess(true);
                showMessage({
                message: "Cadastrado com Sucesso",
                description: "Registro Cadastrado",
                floating: true,
                statusBarHeight: 70,
                type: "success",
                duration: 2000,             
            });         

            } 
        catch (error) {
            console.log("ERRO NO CADASTRO:", error.message);
            showMessage({
                message: "Ops Alguma coisa deu errado, tente novamente.",
                description: res.data.mensagem,
                floating: true,
                statusBarHeight: 70,
                type: "warning",
                duration: 3000,                    
            });
            setSucess(false);  
        }
        
    }   

    function limparCampos(){
        setNome_equipe('');
        setCategoriaEquipe('');
    }


    return(
        <View style={styles.container}>
            <ScrollView contentContainerStyle={styles.scrollContent}>
                <Text style={styles.titulo}>Criar uma equipe</Text>
                <View style={styles.areafotototal}> 
                    <View style={styles.areafoto}>
                    <Image 
                        source={image ? { uri: image } :require('../../../../assets/img/image.png')} 
                        style={styles.imagemPreview} />
                                   
                    <View style={styles.areafoto2}>
                        <TouchableOpacity 
                        style={styles.button} 
                        onPress={pickImageFromGallery}
                        >
                        <Text style={styles.buttonText2}>Escolher da Galeria</Text>
                        </TouchableOpacity>
                
                        <TouchableOpacity 
                        style={styles.button} 
                        onPress={takePhoto}
                        >   
                        <Text style={styles.buttonText2}>Tirar Foto</Text>
                        </TouchableOpacity>   
                    </View>
                    </View>
                </View>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome da Equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite o nome da equipe"
                        value={nome_equipe}
                        onChangeText={setNome_equipe} 
                        placeholderTextColor={theme.text}
                    />
                    <Text style={styles.texto}>Categoria da equipe</Text>
                    <Dropdown
                        style={styles.dropdown}
                        data={dados}
                        labelField="nome_categoria" 
                        valueField="id_categoria"
                        placeholder={categoriaEquipe || "Escolha a categoria da equipe"}
                        placeholderStyle={{ color: theme.text3, fontSize: 14 }}
                        selectedTextStyle={{ color: theme.text, fontSize: 14 }}
                        value={categoriaSelecionada}
                        onChange={item => {
                            setCategoriaSelecionada(item.id_categoria);
                            setCategoriaEquipe(item.nome_categoria);
                        }}
                        containerStyle={{
                            backgroundColor: theme.inputBackground,
                        }}
                        itemTextStyle={{
                            color: theme.text,
                            fontSize: 14,
                            fontFamily: fonts.text,
                        }}
                        selectedStyle={{
                            color: theme.text,
                            fontSize: 14,
                            fontFamily: fonts.text,
                        }}
                        activeColor={theme.inputBackground} 
                    />
                    <TouchableOpacity 
                        style={styles.botaocriar}
                        onPress={cadastrar}
                    >
                        <Text style={styles.textocriar}>Criar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    )
}
