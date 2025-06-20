import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

import { Ionicons } from '@expo/vector-icons';

export default function CadastroEquipes({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const [funcionario, setFuncionario] = useState([
    {
        id: '1',
        nome: 'Gabriel Kenzo Takeuchi',
        datadenascimento: "16/05/1980",
        email: 'kenzo@empresa.com',
        telefone: 13982176670,
        endereco: "Rua João da Fonseca, 123 - Jardim Mato Grosso, Cananeia/SP",
        cargo: "Desenvolvedor Web Sênior",
        equipe: null,
        senha: '1234',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '2',
        nome: 'Leonardo Silva',
        datadenascimento: "22/11/1992",
        email: 'leonardo.silva@empresa.com',
        telefone: 11987654321,
        endereco: "Av. Paulista, 1000 - São Paulo/SP",
        cargo: "Designer UX/UI",
        equipe: null,
        senha: 'abcd123',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '3',
        nome: 'Ana Carolina Oliveira',
        datadenascimento: "05/03/1985",
        email: 'ana.oliveira@empresa.com',
        telefone: 21999887766,
        endereco: "Rua das Flores, 45 - Rio de Janeiro/RJ",
        cargo: "Gerente de Projetos",
        equipe: null,
        senha: 'ana2023',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '4',
        nome: 'Carlos Eduardo Santos',
        datadenascimento: "30/07/1990",
        email: 'carlos.santos@empresa.com',
        telefone: 31988776655,
        endereco: "Av. Afonso Pena, 2000 - Belo Horizonte/MG",
        cargo: "Analista de Dados",
        equipe: null,
        senha: 'carlos123',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '5',
        nome: 'Mariana Costa',
        datadenascimento: "14/09/1988",
        email: 'mariana.costa@empresa.com',
        telefone: 41977665544,
        endereco: "Rua XV de Novembro, 500 - Curitiba/PR",
        cargo: "Product Owner",
        equipe: null,
        senha: 'mari@2023',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '6',
        nome: 'Rafael Pereira',
        datadenascimento: "03/12/1995",
        email: 'rafael.pereira@empresa.com',
        telefone: 51966554433,
        endereco: "Av. Borges de Medeiros, 300 - Porto Alegre/RS",
        cargo: "Desenvolvedor Mobile",
        equipe: null,
        senha: 'rafa1234',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    }
    ]);
    
    const [equipe, setEquipe] = useState([
        {
          id: '1',
          titulo: 'Equipe 1',
          cargo: 'Desenvolvimento de Software',
          membros: null,
          tarefaspostadas: 20,
          quantdeproblemas:6,
          tarefasatrasadas:1,
          tarefasnaoentregues: 6,
          imagem: require('../../../../assets/img/image.png'),
        },
        {
          id: '2',
          titulo: 'Equipe 2',
          cargo: 'Design',
          membros: null,
          tarefaspostadas: 10,
          quantdeproblemas:2,
          tarefasatrasadas:2,
          tarefasnaoentregues: 2,
          imagem: require('../../../../assets/img/image.png'),
        }
      ]);
      
    const [funcionarios, setFuncionarios] = useState(true);
    

    const [nomeEquipe, setNomeEquipe] = useState('');
    const [categoriaEquipe, setCategoriaEquipe] = useState('');
    const [membrosSelecionados, setMembrosSelecionados] = useState([]);

    const [mostrarMembros, setMostrarMembros] = useState(false);
    const [mostrarListaFuncionarios, setMostrarListaFuncionarios] = useState(false);
    const [funcionarioselecionada, setfuncionariosselecionada] = useState(null);


    const cliqueinformacao = () => {
        setMostrarListaFuncionarios(!mostrarListaFuncionarios);
        setFuncionarios(!funcionarios);
        };
    
    const confirmodeequipe = (funcionarioselecionada) => {   
        setfuncionariosselecionada(funcionarioselecionada.nome);
        adicionarFuncionario(funcionarioselecionada)
        setMostrarListaFuncionarios(false); // Fecha a lista após seleção
        };

    const adicionarFuncionario = (funcionario) => {
        if (!membrosSelecionados.includes(funcionario.id)) {
            setMembrosSelecionados([...membrosSelecionados, funcionario.id]);
        }
    };  
    
    const criarEquipe = () => {
    const novaEquipe = {
        id: Date.now().toString(),
        titulo: nomeEquipe,
        cargo: categoriaEquipe,
        membros: membrosSelecionados,
        tarefaspostadas: 0,
        quantdeproblemas: 0,
        tarefasatrasadas: 0,
        tarefasnaoentregues: 0,
        imagem: require('../../../../assets/img/image.png')
    };

        setEquipe([...equipe, novaEquipe]);

        setNomeEquipe('');
        setCategoriaEquipe('');
        setMembrosSelecionados([]);
    };
    
        

    
    return(
        <View style={styles.container}>
            <ScrollView contentContainerStyle={styles.scrollContent}>
                <Text style={styles.titulo}>Criar uma equipe</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome da Equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite o nome da equipe"
                        value={nomeEquipe}
                        onChangeText={setNomeEquipe} 
                        placeholderTextColor={theme.text}
                    />
                    <Text style={styles.texto}>Categoria da equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite a categoria da equipe"
                        value={categoriaEquipe}
                        onChangeText={setCategoriaEquipe} 
                        placeholderTextColor={theme.text}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Adicionar membros à equipe</Text>
                    <View style={styles.linha}>
                        <TouchableOpacity
                            style={styles.inputequipe}
                            onPress={cliqueinformacao}
                        >
                            <View style={styles.linha}>
                                <Text style={styles.textobotao}>{funcionarioselecionada || "Selecione uma funcionario"}</Text> 
                                <Ionicons name="caret-down-outline" size={18} color="black" />     
                            </View>
                        </TouchableOpacity>
                    </View>

                    {mostrarListaFuncionarios && funcionario.map(item => (
                        <TouchableOpacity
                            key={item.id}
                            style={styles.containerequipes}       
                            onPress={() => confirmodeequipe(item)}
                        >
                        <Image source={item.imagem} style={styles.imag} />
                        <View style={styles.textos}>
                            <Text style={styles.textolistatitulo}>{item.nome}</Text>
                            <Text style={styles.textolistacargo}>{item.cargo}</Text>
                        </View>
                        </TouchableOpacity>
                    ))}

                    <Text style={styles.texto}>Membros da Equipe ({membrosSelecionados.length})</Text>

                    {mostrarMembros && membrosSelecionados.map(membroId => {
                        const membro = funcionario.find(f => f.id === membroId);
                            
                        if (!membro) return null;
                        return (
                        <View 
                            key={membroId}
                            style={styles.containerequipes} 
                        >
                            <Image source={membro.imagem} style={styles.imag} />
                            <View style={styles.textos}>
                                <Text style={styles.textolistatitulo}>{membro.nome}</Text>
                                <Text style={styles.textolistacargo}>{membro.cargo}</Text>
                            </View>
                        </View>
                        );
                    })}

                    <TouchableOpacity 
                        style={styles.inputfuncionario}
                        onPress={() => setMostrarMembros(!mostrarMembros)}
                    >
                        <Text style={styles.textobotao3}>{mostrarMembros ? 'Ocultar' : 'Ver mais'}</Text>
                    </TouchableOpacity>

                    <TouchableOpacity 
                        style={styles.botaocriar}
                        onPress={criarEquipe}
                    >
                        <Text style={styles.textocriar}>Criar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    )
}
